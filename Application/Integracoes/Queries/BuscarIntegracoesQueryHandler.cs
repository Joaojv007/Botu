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
    public class BuscarIntegracoesQueryHandler : IBuscarIntegracoesQueryHandler
    {
        private readonly IBotuContext _botuContext;

        public BuscarIntegracoesQueryHandler(IBotuContext botuContext)
        {
            _botuContext = botuContext;
        }

        public List<IntegracaoResponse> Handle(Guid AlunoId)
        {
            var integracoes = _botuContext.Integracoes
                .Include(x => x.Faculdade)
                .Where(x => x.Aluno.Id == AlunoId).ToList();

            var integracoesResponse = new List<IntegracaoResponse>();
            foreach (var integracao in integracoes)
            {
                var response = new IntegracaoResponse();
                response.TipoIntegracao = integracao.TipoIntegracao;
                response.DataIntegracao = integracao.DataIntegracao;
                response.NomeFaculdade = integracao.Faculdade?.Nome;

                integracoesResponse.Add(response);
            }
            return integracoesResponse;
        }
    }
}
