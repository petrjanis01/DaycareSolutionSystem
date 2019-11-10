using System.IdentityModel.Tokens.Jwt;
using Castle.Core.Configuration;
using DaycareSolutionSystem.Api.Host.Controllers.Authentication;
using DaycareSolutionSystem.Database.DataContext;

namespace DaycareSolutionSystem.Api.Host.Services.Authentication
{
    public interface IJwtAuthenticationApiService
    {
        JwtSecurityToken AuthenticateUser(LoginDTO dto);
    }
}
