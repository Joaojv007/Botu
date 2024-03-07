using ApiTcc.Infra.DB;
using ApiTcc.Infra.DB.Entities;
using ApiTcc.Negocio.Enums;
using Application.Interfaces;
using Infra;
using Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Login.Command
{
    public class AdicionarLoginCommand
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class AdicionarLoginCommandHandler : IAdicionarLoginCommandHandler
    {
        private readonly IBotuContext _botuContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public AdicionarLoginCommandHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task Handle(AdicionarLoginCommand command)
        {
            var user = await _botuContext.Users
                .FirstOrDefaultAsync(u => u.Username == command.Login);

            if (user != null)
            {
                throw new System.Exception("Usuário já existe.");
            }

            var newUser = new User
            {
                Username = command.Login,
                // Use o PasswordHasher para criar o hash da senha
                PasswordHash = _passwordHasher.HashPassword(null, command.Senha)
            };

            await _botuContext.Users.AddAsync(newUser);
            _botuContext.SaveChanges();
        }
    }
}
