/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ISimCardsRepository
    {
        Task<(bool IsSuccess, List<SimCardModel>? SimCardList, string? ErrorMessage)> GetAllSimCardDetailsAsync();
        Task<(bool IsSuccess, SimCardModel? SimCardList, string? ErrorMessage)> GetSimCardByIdAsync(Guid Id);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateSimCardAsync(SimCardModel SimCard);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateSimCardAsync(SimCardModel SimCard);
    }
}
