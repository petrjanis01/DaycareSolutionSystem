using System.ComponentModel.DataAnnotations;

namespace DaycareSolutionSystem.Api.Host.Controllers.Authentication
{
    public class LoginDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
