using System;
using System.Linq;
using System.Security.Claims;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services
{
    public class ApiServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly DssDataContext _dataContext;

        public ApiServiceBase(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected User GetCurrentUser()
        {
            var loginName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User currentUser;

            if (string.IsNullOrEmpty(loginName)
                || (currentUser = _dataContext.Users.FirstOrDefault(u => u.LoginName == loginName)) == null)
            {
                throw new DssNoUserFoundException();
            }

            return currentUser;
        }
    }
}
