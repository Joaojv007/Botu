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
    public class NotasController : ControllerBase
    {

        public NotasController()
        {
        }
        
        [HttpGet("GetDisciplinas")]
        public IActionResult GetDisciplinas(
            [FromQuery] Guid semestreId,  
            [FromServices] IBuscarDisciplinasQueryHandler queryHandler)
        {
            try
            {
                var disciplinas = queryHandler.Handle(semestreId);
                return StatusCode(200, disciplinas); ;
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet("GetSemestres")]
        public IActionResult GetSemestres(
            [FromQuery] Guid AlunoId,  
            [FromQuery] Guid CursoId,  
            [FromServices] IBuscarSemestresQueryHandler queryHandler)
        {
            try
            {
                var semestres = queryHandler.Handle(AlunoId, CursoId);
                return StatusCode(200, semestres); ;
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet("GetCursos")]
        public IActionResult GetCursos(
            [FromQuery] Guid AlunoId,  
            [FromServices] IBuscarCursosQueryHandler queryHandler)
        {
            try
            {
                var cursos = queryHandler.Handle(AlunoId);
                return StatusCode(200, cursos); ;
            }
            catch (Exception e)
            {
                return StatusCode(500, HttpStatusCode.InternalServerError);
            }
        }
    }
}