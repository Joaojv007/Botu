namespace Domain.Entities
{
    public class Curso : EntityBase
    {
        public string Nome { get; set; }
        public List<Semestre> Semestres { get; set; }
        public bool IsCursando { get; set; }

    }
}
