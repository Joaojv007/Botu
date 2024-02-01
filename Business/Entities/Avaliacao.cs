using ApiTcc.Negocio.Enums;

namespace ApiTcc.Infra.DB.Entities
{
    public class Avaliacao : EntityBase
    {
        public string Nome { get; set; }
        public DateTime DataEntrega { get; set; }
        public string Conteudo { get; set; }
        public decimal Nota { get; set; }
        public EnumTipoAvaliacao TipoTarefa { get; set; }
    }
}
