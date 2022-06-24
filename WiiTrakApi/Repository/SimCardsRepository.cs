/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
namespace WiiTrakApi.Repository
{
    public class SimCardsRepository : ISimCardsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SimCardsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, List<SimCardModel>? SimCardList, string? ErrorMessage)> GetAllSimCardDetailsAsync()
        {
            var SimCardList = await _dbContext.SimCards
                                .Where(y => y.IsActive)
                                .Select(x => x)
                                .AsNoTracking()
                                .ToListAsync();

            if (SimCardList is not null)
            {
                return (true, SimCardList, null);
            }
            return (false, null, "No SimCard found");
        }
        public async Task<(bool IsSuccess, SimCardModel? SimCardList, string? ErrorMessage)> GetSimCardByIdAsync(Guid Id)
        {

            var SimCardList = await _dbContext.SimCards
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.IsActive && x.Id==Id);

            if (SimCardList is not null)
            {
                return (true, SimCardList, null);
            }
            return (false, null, "No SimCard found");
        }
        public async Task<(bool IsSuccess, List<SimCardModel>? SimCards, string? ErrorMessage)> GetSimCardByTechnicianIdAsync(Guid TechnicianId)
        {
            try
            {
                var simcard = await _dbContext.SimCards
                    .Where(x => x.TechnicianId == TechnicianId)
                    .AsNoTracking().OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                if (simcard.Any())
                {
                    return (true, simcard, null);
                }

                return (false, null, "No SimCard found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateSimCardAsync(SimCardModel SimCard)
        {
            try
            {
                await _dbContext.SimCards.AddAsync(SimCard);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateSimCardAsync(SimCardModel SimCard)
        {
            try
            {

                _dbContext.SimCards.Update(SimCard);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

    }
}
