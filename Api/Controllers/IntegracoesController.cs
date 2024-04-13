using ApiTcc.Negocio.Enums;
using ApiTcc.Responses;
using Application.Integracoes.Command;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiTcc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntegracoesController : ControllerBase
    {

        public IntegracoesController()
        {
        }
        
        [HttpPost()]
        [ActionName("Post")]
        public IActionResult Post(
            [FromBody] AdicionarIntegracaoCommand AdicionarIntegracaoCommand,  
            [FromServices] IAdicionarIntegracaoCommandHandler handler)
        {
            try
            {
                handler.Handle(AdicionarIntegracaoCommand);
                return StatusCode(200, new { statusCode = HttpStatusCode.Created });
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }

        }
        
        [HttpGet()]
        [ActionName("Get")]
        public IActionResult Get(
            [FromQuery] Guid AlunoId,  
            [FromServices] IBuscarIntegracoesQueryHandler queryHandler)
        {
            try
            {
                var integracoes = queryHandler.Handle(AlunoId);
                return Ok(integracoes);
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }
        }
    }
}