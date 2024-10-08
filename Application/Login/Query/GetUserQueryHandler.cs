﻿using Application.Interfaces;
using Infra;
using Infra.Interfaces;
using Infra.Criptografia;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Entities;

namespace Application.Login.Command
{
    public class GetUserCommand
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public Guid AlunoId { get; set; }
    }

    public class GetUserQueryHandler : IGetUserQueryHandler
    {
        private readonly IBotuContext _botuContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthenticationService _authenticationService;

        public GetUserQueryHandler(IBotuContext botuContext, IOptions<JwtSettings> jwtSettings, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _botuContext = botuContext;
            _passwordHasher = new PasswordHasher<User>();
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> Handle(GetUserCommand command)
        {
            var user = await _botuContext.Users.FirstOrDefaultAsync(u => u.Username == command.Login);

            if (user == null || !_authenticationService.VerifyPassword(user, command.Senha))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            var response = new LoginResponse()
            {
                Token = _authenticationService.GenerateJwtToken(user),
                AlunoId = user.AlunoId,
            };

            return response;
        }
    }
}
