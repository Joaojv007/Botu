namespace ApiTcc.Infra.DB.Entities
{
    public class Faculdade : EntityBase
    {
        public string Nome { get; set; }
        public Guid AlunoId { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}
