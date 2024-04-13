using Application.Interfaces;
using Domain.Entities;
using Infra;

namespace Application.Alunos.Queries
{
    public class BuscarAlunoQueryHandler : IBuscarAlunoQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarAlunoQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public Aluno Handle(Guid AlunoId)
        {
            var aluno = _botuContext.Alunos
                .FirstOrDefault(x => x.Id == AlunoId);

            return aluno;
        }
    }
}
