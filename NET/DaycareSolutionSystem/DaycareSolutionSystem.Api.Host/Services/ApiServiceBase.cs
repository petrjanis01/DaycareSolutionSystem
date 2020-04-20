using System;
using System.Linq;
using System.Security.Claims;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities;
using DaycareSolutionSystem.Database.Entities.Entities;
using DaycareSolutionSystem.Helpers;
using Microsoft.AspNetCore.Http;

namespace DaycareSolutionSystem.Api.Host.Services
{
    public class ApiServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly DssDataContext DataContext;

        public ApiServiceBase(DssDataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            DataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected User GetCurrentUser()
        {
            var loginName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User currentUser;

            if (string.IsNullOrEmpty(loginName)
                || (currentUser = DataContext.Users.FirstOrDefault(u => u.LoginName == loginName)) == null)
            {
                throw new DssNoUserFoundException();
            }

            return currentUser;
        }

        public TEntity ChangeProfilePicture<TEntity>(Guid? id, string pictureUri) where TEntity : Person
        {
            var person = DataContext.Find<TEntity>(id);
            var picture = Base64ImageHelper.CreatePictureFromUri(pictureUri);

            if (person != null && picture != null)
            {
                if (person.ProfilePicture != null)
                {
                    person.ProfilePicture.MimeType = picture.MimeType;
                    person.ProfilePicture.BinaryData = picture.BinaryData;
                }
                else
                {
                    person.ProfilePicture = picture;
                    DataContext.Pictures.Add(picture);
                }

                DataContext.SaveChanges();
                return person;
            }

            return null;
        }
    }
}
