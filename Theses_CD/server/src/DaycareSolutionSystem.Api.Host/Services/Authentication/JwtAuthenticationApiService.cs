using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DaycareSolutionSystem.Api.Host.Controllers.Authentication;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Entities.Enums;
using DaycareSolutionSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace DaycareSolutionSystem.Api.Host.Services.Authentication
{
    public class JwtAuthenticationApiService : ApiServiceBase, IJwtAuthenticationApiService
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public JwtAuthenticationApiService(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration config)
        : base(dataContext, httpContextAccessor)
        {
            _config = config;
        }

        public JwtSecurityToken AuthenticateUser(LoginDTO dto)
        {
            var user = DataContext.Users.FirstOrDefault(u => u.LoginName == dto.Username);

            if (user != null && PasswordHasher.HashPassword(dto.Password) == user.Password)
            {
                var userRole = user.Employee.EmployeePosition;

                if (dto.IsManagementSite && userRole != EmployeePosition.Manager)
                {
                    return null;
                }

                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.LoginName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(new IdentityOptions().ClaimsIdentity.RoleClaimType, userRole.ToString())
                };

                var securityKey = _config.GetSection("AppConfiguration")?.GetValue<string>("SecurityKey");
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                var token = new JwtSecurityToken(
                    issuer: "DayCareSolutionSystem",
                    audience: "DayCareSolutionSystemMobileApp",
                    expires: DateTime.Now.AddMinutes(60),
                    claims: authClaims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return token;
            }

            return null;
        }
    }
}
