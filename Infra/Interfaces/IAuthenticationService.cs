using ApiTcc.Negocio.Enums;
using Infra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Interfaces  
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(User user);
        bool VerifyPassword(User user, string providedPassword);
    }
}
