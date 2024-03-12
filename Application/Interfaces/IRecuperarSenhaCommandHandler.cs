using Application.Login.Command;

namespace Application.Interfaces
{
    public interface IRecuperarSenhaCommandHandler
    {
        Task Handle(RecuperarSenhaCommand email);
    }
}
