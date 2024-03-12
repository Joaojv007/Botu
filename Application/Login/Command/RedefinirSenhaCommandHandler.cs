using Application.Interfaces;
using Infra;
using Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Login.Command
{
    public class RedefinirSenhaCommand
    {
        public string Email { get; set; }
        public string NovaSenha { get; set; }
        public string Token { get; set; }
    }

    public class RedefinirSenhaCommandHandler : IRedefinirSenhaCommandHandler
    {
        private readonly IBotuContext _botuContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public RedefinirSenhaCommandHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task Handle(RedefinirSenhaCommand command)
        {
            var aluno = await _botuContext.Alunos.FirstOrDefaultAsync(u => u.Email == command.Email);
            if (aluno != null)
            {
                var user = await _botuContext.Users.FirstOrDefaultAsync(u => u.AlunoId == aluno.Id);
                if (user != null && user.ResetPasswordToken == command.Token && user.ResetPasswordTokenExpiry >= DateTime.UtcNow)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(null, command.NovaSenha);
                    user.ResetPasswordToken = null;
                    user.ResetPasswordTokenExpiry = null;
                    await _botuContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Token inválido ou expirado.");
                }
            }
        }
    }
}
