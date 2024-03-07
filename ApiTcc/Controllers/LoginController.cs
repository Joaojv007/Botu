using ApiTcc.Negocio.Enums;
using ApiTcc.Responses;
using Application.Integracoes.Command;
using Application.Interfaces;
using Application.Login.Command;
using Microsoft.AspNetCore.Authorization;
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
        
        [AllowAnonymous]
        [HttpPost("Cadastro")]
        public async Task<IActionResult> Cadastro(
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(
            [FromBody] GetUserCommand getUserCommand,  
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