using DaycareSolutionSystem.Database.Entities.Entities;

namespace DaycareSolutionSystem.Helpers
{
    public interface IImageFetcherService
    {
        Picture DownloadImageFromUrl(string uri);

        Picture LoadPictureFromPath(string filePath, string mimeType);
    }
}
