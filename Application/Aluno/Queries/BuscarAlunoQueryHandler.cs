using ApiTcc.Infra.DB.Entities;
using ApiTcc.Responses;
using Application.Interfaces;
using Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Alunos.Queries
{
    public class BuscarAlunoQueryHandler : IBuscarAlunoQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarAlunoQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public Aluno Handle(Guid AlunoId)
        {
            var aluno = _botuContext.Alunos
                .FirstOrDefault(x => x.Id == AlunoId);

            return aluno;
        }
    }
}
