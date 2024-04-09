using ApiTcc.Negocio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto_s
{
    public class AvaliacaoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataEntrega { get; set; }
        public string Conteudo { get; set; }
        public decimal Nota { get; set; }
        public EnumTipoAvaliacao TipoTarefa { get; set; }
    }
}
