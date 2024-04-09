namespace ApiTcc.Infra.DB.Entities
{
    public class CursoAluno : EntityBase
    {
        public Curso Faculdade { get; set; }
        public Aluno Aluno { get; set; }
    }
}
