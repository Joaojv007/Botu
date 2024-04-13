using Domain.Entities;

namespace Infra.Interfaces  
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(User user);
        bool VerifyPassword(User user, string providedPassword);
    }
}
