using Application.Interfaces;
using Infra;
using Infra.Interfaces;
using Infra.Criptografia;
using Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Login.Command
{
    public class GetUserCommand
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class GetUserCommandHandler : IGetUserCommandHandler
    {
        private readonly IBotuContext _botuContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthenticationService _authenticationService;

        public GetUserCommandHandler(IBotuContext botuContext, IOptions<JwtSettings> jwtSettings, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _botuContext = botuContext;
            _passwordHasher = new PasswordHasher<User>();
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> Handle(GetUserCommand command)
        {
            var user = await _botuContext.Users.FirstOrDefaultAsync(u => u.Username == command.Login);

            if (user == null || !_authenticationService.VerifyPassword(user, command.Senha))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            return _authenticationService.GenerateJwtToken(user);
        }
    }
}
