using System;
using System.Collections.Generic;
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
                    //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

                    var integracoesPendentesSiga = _botuContext.Integracoes
                        .Include(x => x.Aluno)
                        .Where(x => x.TipoIntegracao == ApiTcc.Negocio.Enums.EnumTipoIntegracao.Siga)
                        .ToList();

                    foreach (var integracao in integracoesPendentesSiga)
                    {
                        _alunoId = integracao.Aluno.Id;
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
                }

                AdicionarSemestreDb(listSemestres);
            }

        }

        private void AdicionarSemestreDb(List<Semestre> listSemestres)
        {
            _botuContext.Semestres.AddRange(listSemestres);
            _botuContext.SaveChanges();
        }

        private List<Dictionary<string, string>> CapturarInformacoesSemestresDropdown()
        {
            // Clicar no ícone para abrir o dropdown
            EsticarDropdown(0);

            Thread.Sleep(1000);

            // Encontrar o painel do dropdown agora que ele está visível
            var dropdownPanel = _driver.FindElement(By.Id("formPrincipal:input_filtro_aca_periodo_letivo_campo_panel"));

            Thread.Sleep(1000);
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

        //private void EsticarDropdown(string dropdown)
        //{
        //    var iconeDropdown = _driver.FindElement(By.CssSelector(dropdown));
        //    iconeDropdown.Click();
        //}

        private void EsticarDropdown(int indice)
        {
            Thread.Sleep(1000);

            // Localizar todos os elementos com a classe iconeCampo.fa.fa-caret-square-o-down
            var elementosDropdown = _driver.FindElements(By.CssSelector("i.iconeCampo.fa.fa-caret-square-o-down"));

            Thread.Sleep(1000);

            // Verificar se foram encontrados elementos
            if (elementosDropdown.Count > 0)
            {
                // Selecionar o primeiro elemento encontrado (ou o que for adequado)
                var iconeDropdown = elementosDropdown[indice];

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

                listSemestre.Add(semestre);
            }

            return listSemestre;
        }

        private List<Dictionary<string, string>> CapturarInformacoesNotaParcial(Semestre semestre)
        {
            EsticarDropdown(0);
            Thread.Sleep(1000);
            var inputDropdown = _driver.FindElement(By.CssSelector("input[title*='Informe o período letivo']"));
            Thread.Sleep(1000);
            inputDropdown.Click();
            Thread.Sleep(1000);
            var opcaoSemestre = _driver.FindElement(By.XPath($"//span[text()='{semestre.Nome}']"));
            opcaoSemestre.Click();

            SelecionarNotaParcialMetodoDiferente();

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

                var infoDisciplina = new Dictionary<string, string>
                {
                    { "Disciplina", disciplina },
                    { "Nota", nota },
                    { "Faltas", faltas }
                };
                informacoes.Add(infoDisciplina);
            }

            return informacoes;
        }

        private void SelecionarNotaParcialMetodoDiferente()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            EsticarDropdown(2);

            js.ExecuteScript("$('tr[data-item-label*=\\\"Nota Parcial\\\"]').click()");
            Thread.Sleep(2000);
            Console.WriteLine("passou SelecionarNotaParcialMetodoDiferente");
        }
        
        private void SelecionarNotaParcialMetodoDiferenteB()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("$('span[class*=\\\"fa fa-search\\\"]').eq(2).click();");
            Thread.Sleep(2000);
            js.ExecuteScript("$('td[role*=\\\"gridcell\\\"]').eq(0).click();");

            //$("span[class*='fa fa-search']")[2].click()
            //$('td[role*="gridcell"]')[0].click()


            Console.WriteLine("passou SelecionarNotaParcialMetodoDiferente");
        }
        
        private void SelecionarNotaParcial()
        {
            EsticarDropdown(2);
            var inputNota = _driver.FindElement(By.CssSelector("input[title*='Informe uma nota para visualizar']"));
            inputNota.Click();

            //WaitForElementToBeClickable(opcaoNotaElemento, 10);

            //Recursiva(true);
            Console.WriteLine("WTF");

        }

        private void Recursiva(bool nsai) 
        {
            while (nsai)
            {
                try
                {
                    //var opcaoNotaElemento = _driver.FindElement(By.CssSelector("tr[data-item-label*='Nota Parcial']"));                    
                    var opcaoNotaElemento = _driver.FindElement(By.XPath("/html/body/div[9]/table/tbody/tr[1]/td"));
                    opcaoNotaElemento.Click();
                    Console.WriteLine("EITA");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao tentar encontrar ou clicar no elemento: {e.Message}");
                    var teste = e;
                    Recursiva(true);
                }
            }
        }

        //private void SelecionarNotaParcial()
        //{
        //    string script = "document.querySelector(\"input[title*='Informe uma nota para visualizar']\").value = 'Media';";
        //    _driver.ExecuteScript(script);
        //    // tentar clicando na lupa mudando de pagina e selecionando
        //    //https://stackoverflow.com/questions/45002008/selenium-stale-element-reference-element-is-not-attached-to-the-page
        //    //https://groups.google.com/g/selenium-users/c/RauvQ6-kbLo
        //}

        private void NavegarTelaNotasFaltas()
        {
            Thread.Sleep(2000);

            var linkElement = _driver.FindElement(By.XPath("//div[@class='ds-painelDeLinks' and @title='Notas e faltas']/a"));
            linkElement.Click();

            Thread.Sleep(5000);
        }

        private void LogarSiga(string login, string senha)
        {
            var campoLogin = _driver.FindElement(By.Id("j_username"));
            campoLogin.SendKeys(login);

            var campoSenha = _driver.FindElement(By.Id("senha"));
            campoSenha.SendKeys(senha);

            var btnEntrar = _driver.FindElement(By.Id("btnLogin"));
            btnEntrar.Click();

            Thread.Sleep(5000);
        }
    }
}
