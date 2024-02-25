using ApiTcc.Infra.DB;
using ApiTcc.Infra.DB.Entities;
using ApiTcc.Negocio.Enums;
using Application.Interfaces;
using Infra;
// Adicione esta referência no seu handler

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

        public AdicionarIntegracaoCommandHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public void Handle(AdicionarIntegracaoCommand AdicionarIntegracaoCommand)
        {
            var aluno = _botuContext.Alunos
                .First(x => x.Id == AdicionarIntegracaoCommand.AlunoId);
            
            var faculdade = _botuContext.Faculdades.First(x => x.Id == AdicionarIntegracaoCommand.FaculdadeId);

            var novaIntegracao = new Integracao
            {
                Aluno = aluno,
                Faculdade = faculdade,
                TipoIntegracao = AdicionarIntegracaoCommand.TipoIntegracao,
                Login = AdicionarIntegracaoCommand.Login,
                Senha = AdicionarIntegracaoCommand.Senha,
                Erro = false,
                ErroDescricao = "",
                DataIntegracao = DateTime.Now
            };

            _botuContext.Integracoes.Add(novaIntegracao);

            _botuContext.SaveChanges();
        }
    }
}

