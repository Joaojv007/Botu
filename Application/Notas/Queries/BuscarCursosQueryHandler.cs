using ApiTcc.Infra.DB.Entities;
using ApiTcc.Responses;
using Application.Interfaces;
using Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Integracoes.Queries
{
    public class BuscarCursosQueryHandler : IBuscarCursosQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarCursosQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public async Task<List<Curso>> Handle(Guid alunoId)
        {
            var faculdadeAluno = await _botuContext.FaculdadeAluno
                .Include(x => x.Faculdade)
                .Include(x => x.Aluno)
                .FirstOrDefaultAsync(x => x.Aluno.Id == alunoId);

            return faculdadeAluno.Faculdade.Cursos;
        }
    }
}
