using System;
using System.Text.RegularExpressions;
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

        // https://stackoverflow.com/questions/5714281/regex-to-parse-image-data-uri/5714347
        protected Picture CreatePictureFromUri(string pictureUri)
        {
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
    }
}
