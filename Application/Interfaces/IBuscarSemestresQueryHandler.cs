using Domain.Dto_s;

namespace Application.Interfaces
{
    public interface IBuscarDisciplinasQueryHandler
    {
        List<DisciplinaDto> Handle(Guid semestreId);
    }
}
