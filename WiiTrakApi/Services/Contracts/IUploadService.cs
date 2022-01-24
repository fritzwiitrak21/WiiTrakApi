namespace WiiTrakApi.Services.Contracts
{
    public interface IUploadService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName);
    }
}
