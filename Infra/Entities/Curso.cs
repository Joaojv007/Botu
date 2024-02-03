namespace ApiTcc.Infra.DB.Entities
{
    public class Curso : EntityBase
    {
        public string Nome { get; set; }
        public List<Semestre> Semestres { get; set; }

    }
}
