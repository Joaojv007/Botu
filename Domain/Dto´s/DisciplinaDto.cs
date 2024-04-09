using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto_s
{
    public class Result 
    { 
        public List<DisciplinaDto> Disciplinas { get; set; }
    }

    public class DisciplinaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Professor { get; set; }
        public double Frequencia { get; set; }
        public int Faltas { get; set; }
        public int Aulas { get; set; }
        public decimal Media { get; set; }
        public List<AvaliacaoDto> Avaliacoes { get; set; } = new List<AvaliacaoDto>();
        public string Resultado { get; set; }
        public Guid SemestreId { get; set; }
    }
}
