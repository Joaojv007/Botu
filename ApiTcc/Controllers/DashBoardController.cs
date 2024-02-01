using Microsoft.AspNetCore.Mvc;

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
    }
}