using Application.Login.Command;

namespace Application.Interfaces
{
    public interface IRedefinirSenhaCommandHandler
    {
        Task Handle(RedefinirSenhaCommand email);
    }
}
