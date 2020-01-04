using System;
using System.Text.RegularExpressions;
using DaycareSolutionSystem.Database.Entities;
using DaycareSolutionSystem.Database.Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DaycareSolutionSystem.Api.Host.Controllers
{
    public class DssBaseController : ControllerBase
    {
        protected string FormatPictureToBase64(Picture picture)
        {
            if (picture == null)
            {
                return null;
            }

            var base64Picture = $"data:{picture.MimeType};base64,{Convert.ToBase64String(picture.BinaryData)}";
            return base64Picture;
        }
    }
}
