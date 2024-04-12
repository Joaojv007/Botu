namespace Domain.Entities
{
    public class FaculdadeAluno : EntityBase
    {
        public Faculdade Faculdade { get; set; }
        public Aluno Aluno { get; set; }
    }
}
