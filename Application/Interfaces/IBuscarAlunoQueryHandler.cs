using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBuscarAlunoQueryHandler
    {
        Aluno Handle(Guid AlunoId);
    }
}
