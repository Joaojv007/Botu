namespace ApiTcc.Infra.DB.Entities
{
    public class Aluno : EntityBase
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public List<FaculdadeAluno> Faculdade { get; set; }
        public List<Integracao> Integracoes { get; set; }
    }
}
