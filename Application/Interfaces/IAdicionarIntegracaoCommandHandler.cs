﻿using ApiTcc.Negocio.Enums;
using Application.Integracoes.Command;
using Application.Login.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGetUserCommandHandler
    {
        Task<string> Handle(GetUserCommand command);
    }
}
