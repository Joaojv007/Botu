using Domain.Dto_s;

namespace Application.Interfaces
{
    public interface IBuscarDisciplinasQueryHandler
    {
        Result Handle(Guid semestreId);
    }
}
