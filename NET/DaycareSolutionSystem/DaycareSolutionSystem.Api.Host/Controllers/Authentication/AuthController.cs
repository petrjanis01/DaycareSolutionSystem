using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DaycareSolutionSystem.Api.Host.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthenticationApiService _jwtAuthenticationService;

        public AuthController(IJwtAuthenticationApiService jwtAuthenticationService)
        {
            _jwtAuthenticationService = jwtAuthenticationService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var token = _jwtAuthenticationService.AuthenticateUser(dto);

            if (token != null)
            {
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }
    }
}