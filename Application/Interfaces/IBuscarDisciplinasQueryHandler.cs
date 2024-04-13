

using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBuscarSemestresQueryHandler
    {
        Task<List<Semestre>> Handle(Guid AlunoId, Guid curso);
    }
}
