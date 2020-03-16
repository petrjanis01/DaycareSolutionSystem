using DaycareSolutionSystem.Api.Host.Controllers.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize(Roles = "Manager")]
        [Route("test")]
        [HttpPost]
        public IActionResult GetTestString(string lat, string lon)
        {
            return Ok();
        }
    }
}