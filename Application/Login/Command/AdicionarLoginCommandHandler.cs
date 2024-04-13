
using Application.Interfaces;
using Domain.Entities;
using Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Login.Command
{
    public class AdicionarLoginCommand
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
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

            var aluno = CriarAluno(command);
            aluno.Faculdade.Add(CriarFaculdadeAluno(aluno));

            var newUser = new User
            {
                Username = command.Login,
                PasswordHash = _passwordHasher.HashPassword(null, command.Senha),
                AlunoId = aluno.Id
            };

            await _botuContext.Alunos.AddAsync(aluno);
            await _botuContext.Users.AddAsync(newUser);
            _botuContext.SaveChanges();
        }

        private Aluno CriarAluno(AdicionarLoginCommand command)
        {
            var id = Guid.NewGuid() ;
            return new Aluno
            {
                Id = id,
                Email = command.Email,
                Nome = command.Nome,
                DataNascimento = command.DataNascimento,
                Integracoes = new List<Integracao>(),
                Faculdade = new List<FaculdadeAluno>(),
            };
        }

        private FaculdadeAluno CriarFaculdadeAluno(Aluno aluno)
        {
            var novaFaculdadeAluno = new FaculdadeAluno
            {
                Aluno = aluno,
                Faculdade = _botuContext.Faculdades.First(),
            };

            return novaFaculdadeAluno;
        }

    }
}
