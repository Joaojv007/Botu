namespace ApiTcc.Infra.DB.Entities
{
    public class Disciplina : EntityBase
    {
        public string Nome { get; set; }
        public string Professor { get; set; }
        public int Frequencia { get; set; }
        public int Faltas { get; set; }
        public int Aulas { get; set; }
        public List<Avaliacao> Avaliacoes { get; set; }
        public int Resultado { get; set; }
    }
}
