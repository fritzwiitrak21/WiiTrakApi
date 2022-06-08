/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class RepairIssueRepository: IRepairIssueRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RepairIssueRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, RepairIssueModel? RepairIssue, string? ErrorMessage)> GetRepairIssueByIdAsync(Guid id)
        {
            var repairIssue = await _dbContext.RepairIssues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (repairIssue is not null)
            {
                return (true, repairIssue, null);
            }
            return (false, null, "No repair issue found");
        }

        public async Task<(bool IsSuccess, List<RepairIssueModel>? RepairIssues, string? ErrorMessage)> GetAllRepairIssuesAsync()
        {
            try
            {
                var issues = await _dbContext.RepairIssues
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (issues.Any())
                {
                    return (true, issues, null);
                }
                return (false, null, "No repair issues found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateRepairIssueAsync(RepairIssueModel repairIssue)
        {
            try
            {
                await _dbContext.RepairIssues.AddAsync(repairIssue);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateRepairIssueAsync(RepairIssueModel repairIssue)
        {
            try
            {
                _dbContext.RepairIssues.Update(repairIssue);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteRepairIssueAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.RepairIssues.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Repair issue not found");
                }
                _dbContext.RepairIssues.Remove(recordToDelete);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }

    }
}
