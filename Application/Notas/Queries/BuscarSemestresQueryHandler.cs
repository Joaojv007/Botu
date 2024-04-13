using Application.Interfaces;
using Domain.Entities;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace Application.Integracoes.Queries
{
    public class BuscarSemestresQueryHandler : IBuscarSemestresQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarSemestresQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public async Task<List<Semestre>> Handle(Guid alunoId, Guid curso)
        {
            var faculdadeAluno = _botuContext.FaculdadeAluno
                .Include(x => x.Faculdade)
                .ThenInclude(x => x.Cursos)
                .ThenInclude(x => x.Semestres)
                .Include(x => x.Aluno)
                .FirstOrDefault(x => x.Aluno.Id == alunoId);

            var retorno = faculdadeAluno.Faculdade.Cursos.FirstOrDefault(x => x.Id == curso).Semestres.ToList();

            return retorno;
        }

    }
}
