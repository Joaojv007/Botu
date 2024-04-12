using Domain.Entities;
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
        public DbSet<User> Users { get; set; }

        public override int SaveChanges() => base.SaveChanges();
        public  Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semestre>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).HasMaxLength(200);
                entity.HasMany(s => s.Disciplinas)
                    .WithOne(d => d.Semestre)
                    .HasForeignKey(d => d.SemestreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Disciplina>(entity =>
            {
                entity.HasMany(d => d.Avaliacoes)
                .WithOne(a => a.Disciplina)
                .HasForeignKey(a => a.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
