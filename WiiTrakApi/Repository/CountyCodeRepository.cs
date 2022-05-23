using Microsoft.EntityFrameworkCore;
using System.Linq;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class CountyCodeRepository : ICountyCodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CountyCodeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, List<CountyCodeModel>? CountyList, string? ErrorMessage)> GetCountyListAsync()
        {
            var CountyCode = await _dbContext.CountyCode
                                .Where(y => y.IsActive)
                                .Select(x => x)
                                .AsNoTracking()
                                .ToListAsync();

            if (CountyCode is not null)
            {
                return (true, CountyCode, null);
            }
            return (false, null, "No countycode found");
        }
    }
}
