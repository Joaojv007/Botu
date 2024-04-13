
using ApiTcc.Responses;

namespace Application.Interfaces
{
    public interface IBuscarIntegracoesQueryHandler
    {
        List<IntegracaoResponse> Handle(Guid AlunoId);
    }
}
