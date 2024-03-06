using System;
using System.Collections.Generic;
using System.Globalization;
using ApiTcc.Infra.DB.Entities;
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
                    var integracoesPendentesSiga = _botuContext.Integracoes
                        .Include(x => x.Aluno)
                        .Include(x => x.Faculdade)
                        .ThenInclude(x => x.Cursos)
                        .Where(x => x.TipoIntegracao == ApiTcc.Negocio.Enums.EnumTipoIntegracao.Siga)
                        .ToList();

                    foreach (var integracao in integracoesPendentesSiga)
                    {
                        _alunoId = integracao.Aluno.Id;
                        _integracao = integracao;
                        ExecutarIntegracao(integracao);
                    }

                    return new GenericCommandResult(true, "Dados coletados com sucesso", "");
                }
            }
            catch (Exception ex)
            {
                return new GenericCommandResult(false, "Erro ao executar o job do SIGA", ex.Message);
            }
        }

        private void ExecutarIntegracao(Integracao integracao)
        {
            if (integracao != null)
            {
                _driver.Navigate().GoToUrl(_url);

                LogarSiga(integracao.Login, integracao.Senha);

                NavegarTelaNotasFaltas();

                var informacoesNotaParcialSemestres = CapturarInformacoesSemestresDropdown();
                var listSemestres = TransferirInformacoesParaSemestres(informacoesNotaParcialSemestres);

                if (integracao.CapturouSemestresPassados)
                    listSemestres.RemoveRange(1, listSemestres.Count - 1);

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
                            Thread.Sleep(500);
                            var informacoesAvaliacoes = CapturarInformacoesAvaliacoes();

                            List<Avaliacao> avaliacoes = TransferirInformacoesParaAvaliacoes(informacoesAvaliacoes);
                            semestre.Disciplinas[i].Avaliacoes = semestre.Disciplinas[i].Avaliacoes == null ? new List<Avaliacao>() : semestre.Disciplinas[i].Avaliacoes;
                            semestre.Disciplinas[i].Avaliacoes.AddRange(avaliacoes);

                            _driver.Navigate().Back();
                            Thread.Sleep(500);
                            IrParaSemestre(semestre);
                            SelecionarNotaParcial();
                            Thread.Sleep(500);
                        }
                    }
                }

                integracao.CapturouSemestresPassados = true;
                AdicionarSemestreDb(listSemestres);
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

        private void AdicionarSemestreDb(List<Semestre> listSemestres)
        {
            foreach (var semestre in listSemestres)
            {
                var semestresExistente = _botuContext.Semestres
                    .Include(x => x.Disciplinas)
                        .ThenInclude(x => x.Avaliacoes)
                    .FirstOrDefault(x => x.Nome == semestre.Nome);

                if (semestresExistente != null)
                {
                    // Atualizar as disciplinas existentes com base nas disciplinas do semestre fornecido
                    foreach (var disciplina in semestre.Disciplinas)
                    {
                        var disciplinaExistente = semestresExistente.Disciplinas.FirstOrDefault(d => d.Id == disciplina.Id);
                        if (disciplinaExistente != null)
                        {
                            // Atualizar as propriedades da disciplina existente
                            disciplinaExistente.Nome = disciplina.Nome;
                            disciplinaExistente.Professor = disciplina.Professor;
                            disciplinaExistente.Frequencia = disciplina.Frequencia;
                            disciplinaExistente.Faltas = disciplina.Faltas;
                            disciplinaExistente.Aulas = disciplina.Aulas;
                            disciplinaExistente.Media = disciplina.Media;

                            // Atualizar as avaliações existentes
                            foreach (var avaliacao in disciplina.Avaliacoes)
                            {
                                var avaliacaoExistente = disciplinaExistente.Avaliacoes.FirstOrDefault(a => a.Id == avaliacao.Id);
                                if (avaliacaoExistente != null)
                                {
                                    // Atualizar as propriedades da avaliação existente
                                    avaliacaoExistente.Nome = avaliacao.Nome;
                                    avaliacaoExistente.DataEntrega = avaliacao.DataEntrega;
                                    avaliacaoExistente.Conteudo = avaliacao.Conteudo;
                                    avaliacaoExistente.Nota = avaliacao.Nota;
                                    avaliacaoExistente.TipoTarefa = avaliacao.TipoTarefa;
                                }
                                else
                                {
                                    // Adicionar uma nova avaliação
                                    disciplinaExistente.Avaliacoes.Add(avaliacao);
                                }
                            }
                        }
                        else
                        {
                            // Adicionar uma nova disciplina
                            semestresExistente.Disciplinas.Add(disciplina);
                        }
                    }

                    semestresExistente.DataInicio = DateTime.Now;
                    _botuContext.Semestres.Update(semestresExistente); // Marcar o semestre existente como modificado
                }
                else
                {
                    semestre.DataFinal = DateTime.Now;
                    _botuContext.Semestres.AddAsync(semestre); // Adicionar um novo semestre
                }
            }

            _botuContext.SaveChanges(); // Salvar as alterações no contexto
        }


        private List<Dictionary<string, string>> CapturarInformacoesSemestresDropdown()
        {
            // Clicar no ícone para abrir o dropdown
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

        private void AdicionarDisciplinaDb(List<Disciplina> listDisciplinas)
        {
            _botuContext.Disciplinas.AddRange(listDisciplinas);
            _botuContext.SaveChanges();
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
                disciplina.Professor = "Nao atribuido";

                listDisciplinas.Add(disciplina);
            }

            return listDisciplinas;
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
                semestre.CursoId = _integracao.Faculdade.Cursos.FirstOrDefault(x => x.IsCursando == true).Id;

                listSemestre.Add(semestre);
            }

            return listSemestre;
        }

        private List<Dictionary<string, string>> CapturarInformacoesNotaParcial(Semestre semestre)
        {
            IrParaSemestre(semestre);

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
            Thread.Sleep(1500);
            var linkElement = _driver.FindElement(By.XPath("//div[@class='ds-painelDeLinks' and @title='Notas e faltas']/a"));
            linkElement.Click();

            Thread.Sleep(500);
        }

        private void LogarSiga(string login, string senha)
        {
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
            Thread.Sleep(500);

            // Localizar todos os elementos com a classe iconeCampo.fa.fa-caret-square-o-down
            var elementosDropdown = _driver.FindElements(By.CssSelector("i.iconeCampo.fa.fa-caret-square-o-down"));

            // Verificar se foram encontrados elementos
            if (elementosDropdown.Count > 0)
            {
                // Selecionar o primeiro elemento encontrado (ou o que for adequado)
                var iconeDropdown = elementosDropdown[indice];

                // Clicar no ícone para esticar o dropdown
                iconeDropdown.Click();
                Thread.Sleep(500);
            }
            else
            {
                // Caso nenhum elemento seja encontrado, você pode lidar com isso de acordo com a lógica do seu programa
                Console.WriteLine("Nenhum elemento iconeCampo.fa.fa-caret-square-o-down encontrado.");
            }
        }
    }
}
