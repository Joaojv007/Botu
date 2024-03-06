using ApiTcc.Infra.DB.Entities;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace ApiTcc.Infra.DB
{
    public class BotuContext : DbContext, IBotuContext
    {
        public BotuContext(DbContextOptions<BotuContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Faculdade> Faculdades { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Semestre> Semestres { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Integracao> Integracoes { get; set; }
        public DbSet<FaculdadeAluno> FaculdadeAluno { get; set; }
        public DbSet<CursoAluno> CursoAluno { get; set; }
        //public DbSet<Comunidade> Comunidade { get; set; }

        public int SaveChanges() => base.SaveChanges();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semestre>()
                .HasMany(s => s.Disciplinas)
                .WithOne(d => d.Semestre)
                .HasForeignKey(d => d.SemestreId) // Chave estrangeira para Semestre
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Disciplina>()
                .HasMany(d => d.Avaliacoes)
                .WithOne(a => a.Disciplina)
                .HasForeignKey(a => a.DisciplinaId) // Chave estrangeira para Disciplina
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
