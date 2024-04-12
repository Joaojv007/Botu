using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBuscarCursosQueryHandler
    {
        Task<List<Curso>> Handle(Guid alunoId);
    }
}
