namespace ApiTcc.Infra.DB.Entities
{
    public class Disciplina : EntityBase
    {
        public string Nome { get; set; }
        public string Professor { get; set; }
        public double Frequencia { get; set; }
        public int Faltas { get; set; }
        public int Aulas { get; set; }
        public decimal Media { get; set; }
        public List<Avaliacao> Avaliacoes { get; set; }
        public string Resultado { get; set; }
        public Guid SemestreId { get; set; }
        public Semestre Semestre { get; set; }

    }
}
