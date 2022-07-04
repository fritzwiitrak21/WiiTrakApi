/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICountyCodeRepository
    {
        Task<(bool IsSuccess, CountyCodeModel? CountyCode, string? ErrorMessage)> GetCountyCodeByIdAsync(Guid id);
        Task<(bool IsSuccess, List<CountyCodeModel>? CountyList, string? ErrorMessage)> GetCountyListAsync();
        Task<(bool IsSuccess, string? ErrorMessage)> CreateCountyCodeAsync(CountyCodeModel code);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCountyCodeAsync(CountyCodeModel code);
    }
}
