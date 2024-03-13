using System;
using System.Collections.Generic;
using System.Globalization;
using ApiTcc.Infra.DB.Entities;
using Hangfire;
using Infra.Email;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Infra.Hangfire.Jobs
{
    public class SigaNotasJob
    {
        private ChromeOptions _options { get; set; }
        private readonly string _url;
        private readonly ChromeDriver _driver;
        private readonly IBotuContext _botuContext;
        private Guid _alunoId;
        private Aluno _aluno;
        private Integracao _integracao;

        public SigaNotasJob(IBotuContext botuContext)
        {
            _botuContext = botuContext;
            _options = new ChromeOptions();
            //_options.AddArgument("--headless");
            _driver = new ChromeDriver(_options);
            _url = "https://siga.udesc.br/sigaSecurityG5/login.jsf?tipoLogin=PADRAO&motivo=SESSAO_EXPIRADA&evento=logout&uri-retorno=login.jsf&execIframe=&codigoSistemaLogout=";
        }

        public async Task<GenericCommandResult> Run()
        {
            try
            {
                using (_driver)
                {
                    var integracoesPendentesSiga = await _botuContext.Integracoes
                        .Include(x => x.Aluno)
                        .Include(x => x.Faculdade)
                        .ThenInclude(x => x.Cursos)
                        .Where(x => x.TipoIntegracao == ApiTcc.Negocio.Enums.EnumTipoIntegracao.Siga)
                        .ToListAsync();

                    foreach (var integracao in integracoesPendentesSiga)
                    {
                        _aluno = integracao.Aluno;
                        _alunoId = _aluno.Id;   
                        _integracao = integracao;
                        var capturouSemestresPassados = _integracao.CapturouSemestresPassados;
                        await ExecutarIntegracao(integracao);

                        if (!capturouSemestresPassados)
                        {
                            BackgroundJob.Enqueue<EmailJob>(job => job.SendEmailAsync(_aluno.Email, "Integração feita com sucesso!", "<!DOCTYPE html> <html> <head>     <style>         body {             font-family: Arial, sans-serif;             margin: 20px;             padding: 0;             background-color: #f4f4f4;             color: #333;         }         .container {             background-color: #fff;             padding: 20px;             border-radius: 5px;             box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);         }         .header {             font-size: 24px;             margin-bottom: 20px;         }         .content {             font-size: 16px;             line-height: 1.6;         }         .footer {             margin-top: 20px;             font-size: 12px;             text-align: center;             color: #aaa;         }     </style> </head> <body>     <div class=\\\"container\\\" >         <div class=\\\"header\\\" > Integração Concluída com Sucesso</div>         <div class=\\\"content\\\" >             <p>Olá,</p>             <p>Estamos felizes em informar que a integração foi concluída com sucesso. Todos os dados necessários foram processados e estão agora atualizados em nosso sistema.</p>             <p>Se você tiver alguma dúvida ou precisar de assistência adicional, por favor, não hesite em entrar em contato conosco.</p>             <p>Atenciosamente,</p>             <p>Equipe Botu</p>         </div>         <div class=\\\"footer\\\">             Este é um e-mail automático, por favor, não responda.         </div>     </div> </body> </html>"));
                        }
                    }

                    return new GenericCommandResult(true, "Dados coletados com sucesso", "");
                }
            }
            catch (Exception ex)
            {
                return new GenericCommandResult(false, "Erro ao executar o job do SIGA", ex.Message);
            }
        }

        private async Task ExecutarIntegracao(Integracao integracao)
        {
            if (integracao != null)
            {
                _driver.Navigate().GoToUrl(_url);

                LogarSiga(integracao.Login, integracao.Senha);

                NavegarTelaNotasFaltas();

                var informacoesNotaParcialSemestres = CapturarInformacoesSemestresDropdown();
                var listSemestres = TransferirInformacoesParaSemestres(informacoesNotaParcialSemestres);

                //if (integracao.CapturouSemestresPassados)
                //    listSemestres.RemoveRange(1, listSemestres.Count - 1);

                //teste
                listSemestres.RemoveRange(2, listSemestres.Count - 2);

                foreach (var semestre in listSemestres)
                {
                    var informacoesNotaParcial = CapturarInformacoesNotaParcial(semestre);
                    semestre.Disciplinas.AddRange(TransferirInformacoesParaDisciplinas(informacoesNotaParcial));

                    var notasParciaisElements = _driver.FindElements(By.CssSelector("a[title*='Clique aqui para visualizar as avaliações parciais que compõem esta nota.']"));
                    for (int i = 0; i < notasParciaisElements.Count; i++)
                    {
                        var infoMedia = informacoesNotaParcial;
                        if (infoMedia[i]["IsLabel"] == "False")
                        {
                            _driver.ExecuteScript($"$('a[title*=\\\"Clique aqui para visualizar as avaliações parciais que compõem esta nota.\\\"]')[{i}].click()");
                            Thread.Sleep(1000);
                            var informacoesAvaliacoes = CapturarInformacoesAvaliacoes();

                            List<Avaliacao> avaliacoes = TransferirInformacoesParaAvaliacoes(informacoesAvaliacoes);
                            semestre.Disciplinas[i].Avaliacoes = semestre.Disciplinas[i].Avaliacoes == null ? new List<Avaliacao>() : semestre.Disciplinas[i].Avaliacoes;
                            semestre.Disciplinas[i].Avaliacoes.AddRange(avaliacoes);

                            _driver.Navigate().GoToUrl("https://siga.udesc.br/sigaMentorWebG5/jsf/dashboard.xhtml");
                            NavegarTelaNotasFaltas();
                            Thread.Sleep(1000);
                            IrParaSemestre(semestre);
                            SelecionarNotaParcial();
                            Thread.Sleep(500);
                        }
                    }
                }

                await AdicionarSemestreDb(listSemestres);
            }

        }

        private List<Avaliacao> TransferirInformacoesParaAvaliacoes(List<Dictionary<string, string>> informacoesAvaliacacoes)
        {
            var listAva = new List<Avaliacao>();
            foreach (var info in informacoesAvaliacacoes)
            {
                var avaliacao = new Avaliacao();
                avaliacao.Nome = info["Avaliação"];

                DateTime data;
                DateTime.TryParseExact(info["Data"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data);                
                avaliacao.DataEntrega = data;

                avaliacao.Conteudo = info["Conteúdo"];
                avaliacao.TipoTarefa = ApiTcc.Negocio.Enums.EnumTipoAvaliacao.Prova;
                avaliacao.Nota = string.IsNullOrEmpty(info["Nota"]) ? 0 : decimal.Parse(info["Nota"]);

                listAva.Add(avaliacao);
            }

            return listAva;
        }

        private async Task AdicionarSemestreDb(List<Semestre> listSemestres)
        {
            foreach (var semestre in listSemestres)
            {
                var semestreExistente = await _botuContext.Semestres
                    .Include(x => x.Disciplinas)
                    .ThenInclude(x => x.Avaliacoes)
                    .FirstOrDefaultAsync(x => x.Nome == semestre.Nome);

                if (semestreExistente != null)
                {
                    if (_integracao.CapturouSemestresPassados)
                        await EnviarEmailAtualizacoesSemestre(semestreExistente, semestre);
                      
                    _botuContext.Semestres.Remove(semestreExistente);
                }
                semestre.DataFinal = DateTime.Now;
                await _botuContext.Semestres.AddAsync(semestre);
            }

            _integracao.CapturouSemestresPassados = true;
            await _botuContext.SaveChangesAsync();
        }

        private async Task EnviarEmailAtualizacoesSemestre(Semestre semestreExistente, Semestre semestreNovo)
        {
            if (semestreExistente.Disciplinas != null)
            {
                foreach (var disciplinaExistente in semestreExistente.Disciplinas)
                {
                    var disciplinaNova = semestreNovo.Disciplinas
                        .FirstOrDefault(d => d.Nome == disciplinaExistente.Nome);

                    if (disciplinaNova != null)
                    {
                        if (disciplinaExistente.Resultado != disciplinaNova.Resultado)
                        {
                            var titulo = $"BotU Atualização Status {disciplinaNova.Nome} !";
                            var body = "<!DOCTYPE html> <html> <head>     <style>         body {             font - family: Arial, sans-serif;             margin: 20px;             padding: 0;             background-color: #f4f4f4;             color: #333;         }         .container {             background - color: #fff;             padding: 20px;             border-radius: 5px;             box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);         }         .header {             font - size: 24px;             margin-bottom: 20px;         }         .content {             font - size: 16px;             line-height: 1.6;         }         .footer {             margin - top: 20px;             font-size: 12px;             text-align: center;             color: #aaa;         }     </style> </head> <body>     <div class=\\\"container\\\" >   <div class=\\\"content\\\" >             <p>Status disciplina " + disciplinaNova.Nome + "</p>      <p>Você está <strong>" + disciplinaNova.Resultado + "</strong>.</p>             <p>Se você tiver alguma dúvida ou precisar de assistência adicional, por favor, não hesite em entrar em contato conosco.</p>             <p>Atenciosamente,</p>             <p>Equipe Botu</p>         </div>         <div class=\\\"footer\\\">             Este é um e-mail automático, por favor, não responda.         </div>     </div> </body> </html>";
                            BackgroundJob.Enqueue<EmailJob>(job => job.SendEmailAsync(_aluno.Email, titulo, body));
                        }

                        VerificarAtualizacoesAvaliacoes(disciplinaExistente, disciplinaNova);
                    }
                }
            }
        }

        private void VerificarAtualizacoesAvaliacoes(Disciplina discilplinaExistentes, Disciplina disciplinaoNova)
        {
            foreach (var avaliacaoNova in disciplinaoNova.Avaliacoes)
            {
                var avaliacaoExiste = discilplinaExistentes.Avaliacoes
                .FirstOrDefault(d => d.Nome == avaliacaoNova.Nome);

                if (avaliacaoExiste == null)
                {
                    var titulo = $"BotU Adição Nota {disciplinaoNova.Nome} !";
                    var body = "<!DOCTYPE html> <html> <head>     <style>         body {             font - family: Arial, sans-serif;             margin: 20px;             padding: 0;             background-color: #f4f4f4;             color: #333;         }         .container {             background - color: #fff;             padding: 20px;             border-radius: 5px;             box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);         }         .header {             font - size: 24px;             margin-bottom: 20px;         }         .content {             font - size: 16px;             line-height: 1.6;         }         .footer {             margin - top: 20px;             font-size: 12px;             text-align: center;             color: #aaa;         }     </style> </head> <body>     <div class=\\\"container\\\" >   <div class=\\\"content\\\" >             <p>Nota adicionada, Avaliação:" + avaliacaoNova.Nome + "</p>      <p>Nota: <strong>" + avaliacaoNova.Nota + "</strong>.</p>             <p>Se você tiver alguma dúvida ou precisar de assistência adicional, por favor, não hesite em entrar em contato conosco.</p>             <p>Atenciosamente,</p>             <p>Equipe Botu</p>         </div>         <div class=\\\"footer\\\">             Este é um e-mail automático, por favor, não responda.         </div>     </div> </body> </html>";
                    BackgroundJob.Enqueue<EmailJob>(job => job.SendEmailAsync(_aluno.Email, titulo, body));
                }
            }
        }

        private List<Dictionary<string, string>> CapturarInformacoesSemestresDropdown()
        {
            EsticarDropdown(0);

            // Encontrar o painel do dropdown agora que ele está visível
            var dropdownPanel = _driver.FindElement(By.Id("formPrincipal:input_filtro_aca_periodo_letivo_campo_panel"));

            // Verificar se o painel dropdown está visível
            if (dropdownPanel.Displayed)
            {
                var opcoes = dropdownPanel.FindElements(By.CssSelector("tr.ui-autocomplete-item"));
                var semestres = new List<Dictionary<string, string>>();

                foreach (var opcao in opcoes)
                {
                    var valor = opcao.GetAttribute("data-item-value");
                    var label = opcao.GetAttribute("data-item-label");

                    var semestreInfo = new Dictionary<string, string>
                    {
                        { "Valor", valor },
                        { "Label", label }
                    };
                    semestres.Add(semestreInfo);
                }
                return semestres;
            }
            else
            {
                Console.WriteLine("Dropdown não está visível após o clique.");
                return new List<Dictionary<string, string>>();
            }
        }

        private List<Disciplina> TransferirInformacoesParaDisciplinas(List<Dictionary<string, string>> informacoes)
        {
            var listDisciplinas = new List<Disciplina>();
            foreach (var info in informacoes)
            {
                var disciplina = new Disciplina();
                disciplina.Nome = info["Disciplina"];
                disciplina.Media = string.IsNullOrEmpty(info["Nota"]) ? 0 : decimal.Parse(info["Nota"]);

                var arrayFaltaAula = info["Faltas"].Split('/');
                disciplina.Faltas = int.TryParse(arrayFaltaAula[0], out int faltasOut) ? faltasOut : 0;
                disciplina.Aulas = int.TryParse(arrayFaltaAula[1], out int aulasOut) ? aulasOut : 0;
                disciplina.Professor = "Não atribuido";
                disciplina.Resultado = "Não atribuído";
                disciplina.Frequencia = CalcularFrequencia(disciplina.Faltas, disciplina.Aulas);

                listDisciplinas.Add(disciplina);
            }

            return listDisciplinas;
        }

        public static double CalcularFrequencia(int faltas, int totalAulas)
        {
            if (totalAulas == 0)
            {
                return 0;
            }
            else if (faltas == 0)
            {
                return 100;
            }
            else
            {
                return (1 - (double)faltas / totalAulas) * 100;
            }
        }

        private List<Semestre> TransferirInformacoesParaSemestres(List<Dictionary<string, string>> informacoes)
        {
            var listSemestre = new List<Semestre>();
            foreach (var info in informacoes)
            {
                var semestre = new Semestre();
                semestre.Nome = info["Label"];
                semestre.AlunoId = _alunoId;
                semestre.Disciplinas = new List<Disciplina>();
                semestre.CursoId = _integracao.Faculdade.Cursos.First(x => x.IsCursando == true).Id;

                listSemestre.Add(semestre);
            }

            return listSemestre;
        }

        private List<Dictionary<string, string>> CapturarInformacoesNotaParcial(Semestre semestre)
        {
            IrParaSemestre(semestre);
            //var informacoesNotaParcial = CapturarDisciplinasResultado();
            SelecionarNotaParcial();

            var tabela = _driver.FindElement(By.Id("formPrincipal:notasFaltas_data"));
            var linhas = tabela.FindElements(By.TagName("tr"));

            var informacoes = new List<Dictionary<string, string>>();

            for (int i = 0; i < linhas.Count; i++)
            {
                var linha = linhas[i];
                var colunas = linha.FindElements(By.TagName("td"));

                var disciplina = colunas[0].Text;
                var nota = colunas[1].Text;
                var faltas = colunas[2].Text;

                var notaElement = colunas[1]; // Tenta encontrar um link dentro da coluna
                var innerHTML = notaElement.GetAttribute("innerHTML"); // Tenta encontrar um link dentro da coluna

                var infoDisciplina = new Dictionary<string, string>
                {
                    { "Disciplina", disciplina },
                    { "Nota", nota },
                    { "Faltas", faltas },
                    { "IsLabel", innerHTML.Contains("</label>").ToString() } // Adiciona o tipo de nota (Link ou Label) ao dicionário
                };
                informacoes.Add(infoDisciplina);
            }

            return informacoes;
        }

        private object CapturarDisciplinasResultado()
        {
            throw new NotImplementedException();
        }

        private void IrParaSemestre(Semestre semestre)
        {
            EsticarDropdown(0);
            var inputDropdown = _driver.FindElement(By.CssSelector("input[title*='Informe o período letivo']"));
            inputDropdown.Click();
            Thread.Sleep(500);
            var opcaoSemestre = _driver.FindElement(By.XPath($"//span[text()='{semestre.Nome}']"));
            opcaoSemestre.Click();
        }

        public List<Dictionary<string, string>> CapturarInformacoesAvaliacoes()
        {
            var tabela = _driver.FindElement(By.Id("formPrincipal:notasParciais_data"));
            var linhas = tabela.FindElements(By.TagName("tr"));

            var informacoes = new List<Dictionary<string, string>>();

            foreach (var linha in linhas)
            {
                var colunas = linha.FindElements(By.TagName("td"));

                var avaliacao = colunas[0].Text;
                var data = colunas[1].Text;
                var conteudo = colunas[2].Text;
                var nota = colunas[3].Text;

                var infoNotaParcial = new Dictionary<string, string>
                {
                    { "Avaliação", avaliacao },
                    { "Data", data },
                    { "Conteúdo", conteudo },
                    { "Nota", nota }
                };
                informacoes.Add(infoNotaParcial);
            }

            return informacoes;
        }

        private void SelecionarNotaParcial()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            EsticarDropdown(2);

            js.ExecuteScript("$('tr[data-item-label*=\\\"Nota Parcial\\\"]').click()");
            Thread.Sleep(500);
        }

        private void NavegarTelaNotasFaltas()
        {
            Thread.Sleep(2000);
            var linkElement = _driver.FindElement(By.XPath("//div[@class='ds-painelDeLinks' and @title='Notas e faltas']/a"));
            Thread.Sleep(500);
            linkElement.Click();
            Thread.Sleep(500);
        }

        private void LogarSiga(string login, string senha)
        {
            Thread.Sleep(500);
            var campoLogin = _driver.FindElement(By.Id("j_username"));
            campoLogin.SendKeys(login);

            var campoSenha = _driver.FindElement(By.Id("senha"));
            campoSenha.SendKeys(senha);

            var btnEntrar = _driver.FindElement(By.Id("btnLogin"));
            btnEntrar.Click();

            Thread.Sleep(500);
        }

        private void EsticarDropdown(int indice)
        {
            Thread.Sleep(1000);
            // Localizar todos os elementos com a classe iconeCampo.fa.fa-caret-square-o-down
            var elementosDropdown = _driver.FindElements(By.CssSelector("i.iconeCampo.fa.fa-caret-square-o-down"));

            // Verificar se foram encontrados elementos
            if (elementosDropdown.Count > 0)
            {
                // Selecionar o primeiro elemento encontrado (ou o que for adequado)
                var iconeDropdown = elementosDropdown[indice];

                Thread.Sleep(2000);
                // Clicar no ícone para esticar o dropdown
                iconeDropdown.Click();
                Thread.Sleep(1000);
            }
            else
            {
                // Caso nenhum elemento seja encontrado, você pode lidar com isso de acordo com a lógica do seu programa
                Console.WriteLine("Nenhum elemento iconeCampo.fa.fa-caret-square-o-down encontrado.");
            }
        }
        
        private void EsticarDropdownJS(int indice)
        {
            Thread.Sleep(500);
            // Localizar todos os elementos com a classe iconeCampo.fa.fa-caret-square-o-down
            var elementosDropdown = _driver.ExecuteScript("$('i[class*=\\\"iconeCampo fa fa - caret - square - o - down\\\"]')");

            // Verificar se foram encontrados elementos
            if (indice == 0)
            {
                _driver.ExecuteScript("$('#formPrincipal\\\\:input_filtro_aca_periodo_letivo_idd')[0].click();");
            }

            if (indice == 1)
            {
                _driver.ExecuteScript("$(\\\"#formPrincipal\\:input_filtro_aca_periodo_letivo_idd\\\").click();");
            }

            if (indice == 2)
            {
                _driver.ExecuteScript("$(\\\"#formPrincipal\\:input_filtro_aca_periodo_letivo_idd\\\").click();");
            }
        }
    }
}
