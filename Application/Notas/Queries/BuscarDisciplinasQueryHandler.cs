using ApiTcc.Infra.DB.Entities;
using ApiTcc.Responses;
using Application.Interfaces;
using Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Integracoes.Queries
{
    public class BuscarDisciplinasQueryHandler : IBuscarDisciplinasQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarDisciplinasQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public async Task<List<Disciplina>> Handle(Guid AlunoId, Guid Semestre)
        {
            var semestre = _botuContext.Semestres
                .Include(x => x.Disciplinas)
                .ThenInclude(x => x.Avaliacoes)
                .FirstOrDefault(x => x.Id == Semestre);

            return semestre.Disciplinas;
        }
    }
}
