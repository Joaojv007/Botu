using ApiTcc.Infra.DB;
using ApiTcc.Infra.DB.Entities;
using ApiTcc.Negocio.Enums;
using Application.Interfaces;
using Hangfire;
using Infra;
using Infra.Entities;
using Infra.Hangfire.Jobs;
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
    public class RecuperarSenhaCommand
    {
        public string Email { get; set; }
    }

    public class RecuperarSenhaCommandHandler : IRecuperarSenhaCommandHandler
    {
        private readonly IBotuContext _botuContext;

        public RecuperarSenhaCommandHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public async Task Handle(RecuperarSenhaCommand command)
        {
            var aluno = await _botuContext.Alunos.FirstOrDefaultAsync(u => u.Email == command.Email);
            if (aluno != null)
            {
                var user = await _botuContext.Users.FirstOrDefaultAsync(u => u.AlunoId == aluno.Id);
                if (user != null)
                {
                    var token = Guid.NewGuid().ToString();
                    user.ResetPasswordToken = token;
                    user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(24);

                    var link = $"http://localhost:4200/dashboard/redefinir-senha?token={token}";

                    BackgroundJob.Enqueue<EmailJob>(job => job.SendEmailAsync(command.Email, "BotU: Redefinição de Senha", "<!DOCTYPE html> <html> <head>     <title>BotU: Redefinição de Senha</title>     <style>         body {             font-family: Arial, sans-serif;             margin: 0;             padding: 20px;             background-color: #f4f4f4;             color: #333;         }         .container {             max-width: 600px;             margin: auto;             background: #fff;             padding: 20px;             border-radius: 8px;             box-shadow: 0 2px 4px rgba(0,0,0,0.1);         }         h1 {             color: #444;         }         a {             background-color: #007bff;             color: #ffffff;             padding: 10px 20px;             text-decoration: none;             border-radius: 5px;         }         a:hover {             background-color: #0056b3;         }     </style> </head> <body>     <div class=\\\"container\\\">         <h1>Redefinição de Senha</h1>         <p>Olá,</p>         <p>Você solicitou a redefinição da sua senha. Por favor, clique no link abaixo para definir uma nova senha:</p>         <p><a href= " + link + " target=\\\"_blank\\\">Redefinir minha senha</a></p>         <p>Se você não solicitou a redefinição da senha, por favor, ignore este e-mail.</p>         <p>Obrigado,</p>         <p>Equipe BotU suporte ao cliente</p>     </div> </body> </html>"));

                    await _botuContext.SaveChangesAsync();
                }
            }
        }
    }
}
