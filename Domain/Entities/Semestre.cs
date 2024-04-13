namespace Domain.Entities
{
    public class Semestre : EntityBase
    {
        public string Nome { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
    }
}
