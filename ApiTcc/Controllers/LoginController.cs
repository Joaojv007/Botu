using ApiTcc.Negocio.Enums;
using ApiTcc.Responses;
using Application.Integracoes.Command;
using Application.Interfaces;
using Application.Login.Command;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiTcc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        public LoginController()
        {
        }
        
        [HttpPost()]
        [ActionName("Post")]
        public async Task<IActionResult> Post(
            [FromBody] AdicionarLoginCommand AdicionarLoginCommand,  
            [FromServices] IAdicionarLoginCommandHandler handler)
        {
            try
            {
                await handler.Handle(AdicionarLoginCommand);
                return StatusCode(200, new { statusCode = HttpStatusCode.Created });
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }

        }
        
        [HttpGet()]
        [ActionName("Get")]
        public async Task<IActionResult> Get(
            [FromQuery] GetUserCommand getUserCommand,  
            [FromServices] IGetUserCommandHandler queryHandler)
        {
            try
            {
                var jwt = await queryHandler.Handle(getUserCommand);
                return Ok(jwt);
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }
        }
    }
}