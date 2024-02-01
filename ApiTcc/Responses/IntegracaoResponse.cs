using ApiTcc.Negocio.Enums;

namespace ApiTcc.Responses
{
    public class IntegracaoResponse
    {
        public string NomeFaculdade { get; set; }
        public DateTime DataIntegracao { get; set; }
        public EnumTipoIntegracao TipoIntegracao { get; set; }
    }
}
