/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.Services.Contracts
{
    public interface IUploadService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName, bool IsCoords = false);
    }
}
