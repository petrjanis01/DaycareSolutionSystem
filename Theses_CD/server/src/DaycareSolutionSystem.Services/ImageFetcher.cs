using System;
using System.IO;
using System.Net;
using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Helpers
{
    public class ImageFetcher
    {

        public Picture DownloadImageFromUrl(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Moved ||
                 response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                var picture = new Picture();
                picture.MimeType = response.ContentType;

                using (Stream inputStream = response.GetResponseStream())
                {
                    var data = LoadImageDataFromStream(inputStream);

                    picture.BinaryData = data;
                }

                return picture;
            }

            return null;
        }

        public Picture LoadPictureFromPath(string filePath, string mimeType)
        {
            var profilePicture = new Picture();
            profilePicture.MimeType = mimeType;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var data = LoadImageDataFromStream(fileStream);
                profilePicture.BinaryData = data;
            }

            return profilePicture;
        }

        private byte[] LoadImageDataFromStream(Stream stream)
        {
            byte[] imageData;

            using (var binaryReader = new BinaryReader(stream))
            using (var memoryStream = new MemoryStream())
            {
                var buffer = new byte[2048];
                int byteCount;
                while ((byteCount = binaryReader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memoryStream.Write(buffer, 0, byteCount);
                }

                imageData = memoryStream.ToArray();
            }

            return imageData;
        }
    }
}
