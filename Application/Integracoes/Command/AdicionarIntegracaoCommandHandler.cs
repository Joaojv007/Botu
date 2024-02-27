using ApiTcc.Infra.DB;
using ApiTcc.Infra.DB.Entities;
using ApiTcc.Negocio.Enums;
using Application.Interfaces;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace Application.Integracoes.Command
{
    public class AdicionarIntegracaoCommand 
    {
        public Guid AlunoId { get; set; }
        public Guid FaculdadeId { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public EnumTipoIntegracao TipoIntegracao { get; set; }

    }

    public class AdicionarIntegracaoCommandHandler : IAdicionarIntegracaoCommandHandler
    {
        private readonly IBotuContext _botuContext;
        private AdicionarIntegracaoCommand _command { get; set; }

        public AdicionarIntegracaoCommandHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public void Handle(AdicionarIntegracaoCommand AdicionarIntegracaoCommand)
        {
            _command = AdicionarIntegracaoCommand;

            var aluno = _botuContext.Alunos
                .Include(x => x.Integracoes)
                .First(x => x.Id == AdicionarIntegracaoCommand.AlunoId);
            
            var faculdade = _botuContext.Faculdades.First(x => x.Id == AdicionarIntegracaoCommand.FaculdadeId);

            var integracao = aluno.Integracoes.First(x => x.TipoIntegracao == _command.TipoIntegracao);

            if (integracao != null)
                UpdateIntegracao(integracao);
            else
                InsertIntegracao(aluno, faculdade);


            _botuContext.SaveChanges();
        }

        private void UpdateIntegracao(Integracao integracao)
        {
            integracao.Login = _command.Login;
            integracao.Senha = _command.Senha;
            _botuContext.Integracoes.Update(integracao);
        }

        private void InsertIntegracao(Aluno aluno, Faculdade faculdade)
        {
            var novaIntegracao = new Integracao
            {
                Aluno = aluno,
                Faculdade = faculdade,
                TipoIntegracao = _command.TipoIntegracao,
                Login = _command.Login,
                Senha = _command.Senha,
                Erro = false,
                ErroDescricao = "",
                DataIntegracao = DateTime.Now
            };

            _botuContext.Integracoes.Add(novaIntegracao);
        }
    }
}

