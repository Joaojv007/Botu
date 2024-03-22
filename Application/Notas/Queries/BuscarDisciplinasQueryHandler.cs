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
using Domain.Dto_s;

namespace Application.Integracoes.Queries
{
    public class BuscarDisciplinasQueryHandler : IBuscarDisciplinasQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarDisciplinasQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public Result Handle(Guid semestre)
        {
            var disciplinas = _botuContext.Disciplinas
                .Where(d => d.SemestreId == semestre)
                .Include(d => d.Avaliacoes)
                .Select(d => new DisciplinaDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Professor = d.Professor,
                    Frequencia = d.Frequencia,
                    Faltas = d.Faltas,
                    Aulas = d.Aulas,
                    Media = d.Media,
                    Resultado = d.Resultado,
                    SemestreId = d.SemestreId,
                    Avaliacoes = d.Avaliacoes.Select(a => new AvaliacaoDto
                    {
                        Id = a.Id,
                        Nome = a.Nome,
                        DataEntrega = a.DataEntrega,
                        Conteudo = a.Conteudo,
                        Nota = a.Nota,
                        TipoTarefa = a.TipoTarefa
                    }).ToList()
                }).ToList();

            var retorno = new Result();
            retorno.Disciplinas = disciplinas;
            return retorno;
        }
    }
}
