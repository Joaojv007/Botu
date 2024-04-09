using ApiTcc.Infra.DB.Entities;
using Application.Interfaces;
using Infra;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiTcc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashBoardController : ControllerBase
    {
        public DashBoardController(){}
        
        [HttpGet("afazeres-pendentes",Name = "GetAfazeresPendentes")]
        [ActionName("GetAfazeresPendentes")]
        public int GetAfazeresPendentes()
        {
            return Random.Shared.Next(0, 10);
        }
        
        [HttpGet("get-aluno",Name = "GetAluno")]
        [ActionName("GetAluno")]
        public GenericCommandResult GetAluno([FromQuery] Guid AlunoId,
            [FromServices] IBuscarAlunoQueryHandler handler)
        {
            try
            {
                var aluno = handler.Handle(AlunoId);
                return new GenericCommandResult(true, "Aluno retornado com sucesso", aluno);

            }
            catch (Exception e)
            {
                return new GenericCommandResult(false, "Erro ao buscar Aluno", e.Message);
            }
        }
    }
}