using ApiTcc.Negocio.Enums;

namespace ApiTcc.Infra.DB.Entities
{
    public class Integracao : EntityBase
    {
        public Aluno Aluno { get; set; }
        public Faculdade Faculdade { get; set; }
        public EnumTipoIntegracao TipoIntegracao { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string ErroDescricao { get; set; }
        public bool Erro { get; set; }
        public bool CapturouSemestresPassados { get; set; }
        public DateTime DataIntegracao { get; set; }
    }
}
