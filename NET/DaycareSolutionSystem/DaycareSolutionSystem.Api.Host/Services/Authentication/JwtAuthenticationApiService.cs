﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Castle.Core.Configuration;
using DaycareSolutionSystem.Api.Host.Controllers.Authentication;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Services;
using Microsoft.IdentityModel.Tokens;

namespace DaycareSolutionSystem.Api.Host.Services.Authentication
{
    public class JwtAuthenticationApiService : IJwtAuthenticationApiService
    {
        private readonly IPasswordHashService _passwordHashService;
        private readonly DssDataContext _dataContext;

        public JwtAuthenticationApiService(IPasswordHashService passwordHashService, DssDataContext dataContext)
        {
            _passwordHashService = passwordHashService;
            _dataContext = dataContext;
        }

        public JwtSecurityToken AuthenticateUser(LoginDTO dto)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.LoginName == dto.Username);

            if (user != null && _passwordHashService.HashPassword(dto.Password) == user.Password)
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.LoginName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // TODO: move to config
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJzdWIiOiJkY2VtcCIsImp0aSI6IjI2YTIwNWM1LTRhYzEtNDVmYS1hZDQxLTk2MjNiY2UzMTBiNiIsImV4cCI6"));

                var token = new JwtSecurityToken(
                    issuer: "DayCareSolutionSystem",
                    audience: "DayCareSolutionSystemMobileApp",
                    expires: DateTime.Now.AddMinutes(30),
                    claims: authClaims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return token;
            }

            return null;
        }
    }
}
