using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Helpers
{
    public static class Base64ImageHelper
    {
        // https://stackoverflow.com/questions/5714281/regex-to-parse-image-data-uri/5714347
        public static Picture CreatePictureFromUri(string pictureUri)
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
    }
}
