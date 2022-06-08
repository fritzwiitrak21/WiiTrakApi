/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICountyCodeRepository
    {
        Task<(bool IsSuccess, List<CountyCodeModel>? CountyList, string? ErrorMessage)> GetCountyListAsync();
    }
}
