using System;
using System.Collections.Generic;
using ApiTcc.Infra.DB.Entities;
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
                    var integracoesPendentesSiga = _botuContext.Integracoes.Where(x => x.TipoIntegracao == ApiTcc.Negocio.Enums.EnumTipoIntegracao.Siga).ToList();
                    foreach (var integracao in integracoesPendentesSiga)
                    {
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
                var informacoes = CapturarInformacoesNotaParcial();

                var listDisciplinas = TransferirInformacoesParaDisciplinas(informacoes);
                AdicionarDisciplinaDb(listDisciplinas);
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

                listDisciplinas.Add(disciplina);
            }

            return listDisciplinas;
        }

        private List<Dictionary<string, string>> CapturarInformacoesNotaParcial()
        {
            // Localizar a tabela de notas e faltas
            var tabela = _driver.FindElement(By.Id("formPrincipal:notasFaltas_data"));

            // Encontrar todas as linhas da tabela
            var linhas = tabela.FindElements(By.TagName("tr"));

            var informacoes = new List<Dictionary<string, string>>();

            // Iterar sobre as linhas da tabela, começando da segunda linha (pois a primeira é o cabeçalho)
            for (int i = 0; i < linhas.Count; i++)
            {
                var linha = linhas[i];

                // Encontrar os elementos de cada coluna na linha atual
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
