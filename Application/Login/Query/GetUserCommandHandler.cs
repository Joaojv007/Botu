using ApiTcc.Infra.DB;
using ApiTcc.Infra.DB.Entities;
using ApiTcc.Negocio.Enums;
using Application.Interfaces;
using Infra;
using Infra.Criptografia;
using Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public GetUserCommandHandler(IBotuContext botuContext, IOptions<JwtSettings> jwtSettings)
        {
            _botuContext = botuContext;
            _passwordHasher = new PasswordHasher<User>();
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> Handle(GetUserCommand command)
        {
            var user = await _botuContext.Users.FirstOrDefaultAsync(u => u.Username == command.Login);

            if (user == null || !VerifyPassword(user, command.Senha))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(User user, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
