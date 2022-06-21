/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using WiiTrakApi.Services.Contracts;

namespace WiiTrakApi.Services
{
    public class UploadService : IUploadService
    {
        private readonly string _storageConnectionString;
        public UploadService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetConnectionString("AzureStorage");
        }
        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName, bool IsCoords = false)
        {
            var container = new BlobContainerClient(_storageConnectionString, containerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
            {
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            }
            var blob = container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            if (IsCoords)
            {
                fileStream.Position = 0;
            }
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            return blob.Uri.ToString();
        }


    }
}
