using System.ComponentModel.DataAnnotations;

namespace DaycareSolutionSystem.Api.Host.Controllers.Authentication
{
    public class LoginDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsManagementSite { get; set; }
    }
}
