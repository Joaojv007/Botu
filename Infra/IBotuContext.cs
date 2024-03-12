using ApiTcc.Infra.DB.Entities;
using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public interface IBotuContext
    {
        DbSet<Aluno> Alunos { get; set; }
        DbSet<Faculdade> Faculdades { get; set; }
        DbSet<Curso> Cursos { get; set; }
        DbSet<Semestre> Semestres { get; set; }
        DbSet<Disciplina> Disciplinas { get; set; }
        DbSet<Avaliacao> Avaliacoes { get; set; }
        DbSet<Integracao> Integracoes { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<FaculdadeAluno> FaculdadeAluno { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
