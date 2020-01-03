using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using DaycareSolutionSystem.Database.DataContext;
using DaycareSolutionSystem.Database.Entities;
using DaycareSolutionSystem.Database.Entities.Entities;
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

        // https://stackoverflow.com/questions/5714281/regex-to-parse-image-data-uri/5714347
        protected Picture CreatePictureFromUri(string pictureUri)
        {
            if (string.IsNullOrEmpty(pictureUri))
            {
                return null;
            }

            var regex = new Regex(@"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)", RegexOptions.Compiled);

            var match = regex.Match(pictureUri);

            var mime = match.Groups["mime"].Value;
            var encoding = match.Groups["encoding"].Value;
            var data = match.Groups["data"].Value;

            var binaryData = Convert.FromBase64String(data);

            var picture = new Picture();
            picture.MimeType = mime;
            picture.BinaryData = binaryData;

            return picture;
        }

        public TEntity ChangeProfilePicture<TEntity>(Guid? id, string pictureUri) where TEntity : Person
        {
            var person = DataContext.Find<TEntity>(id);
            var picture = CreatePictureFromUri(pictureUri);

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
