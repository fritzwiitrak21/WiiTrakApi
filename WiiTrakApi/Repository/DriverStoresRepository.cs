using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using WiiTrakApi.Services;
using WiiTrakApi.Controllers;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Repository
{
    public class DriverStoresRepository : IDriverStoresRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DriverStoresRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(bool IsSuccess, List<SpGetDriverAssignedStoresByCompany>? DriverStores, string? ErrorMessage)> GetDriverStoresByCompanyIdAsync(Guid DriverId,Guid CompanyId)
        {
            try
            {
                string sqlquery = "Exec SpGetDriverAssignedStoresByCompany @DriverId,@CompanyId";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@DriverId", Value = DriverId },
                     new SqlParameter { ParameterName = "@CompanyId", Value = CompanyId }
                };

                var DriverStores= await _dbContext.SpGetDriverAssignedStoresByCompany.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (DriverStores != null)
                {
                    return (true, DriverStores, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<SpGetDriverAssignedStoresBySystemOwner>? DriverStores, string? ErrorMessage)> GetDriverStoresBySystemOwnerIdAsync(Guid DriverId, Guid SystemOwnerId)
        {
            try
            {
                string sqlquery = "Exec SpGetDriverAssignedStoresBySystemOwner @DriverId,@SystemOwnerId";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@DriverId", Value = DriverId },
                     new SqlParameter { ParameterName = "@SystemOwnerId", Value = SystemOwnerId }
                };

                var DriverStores = await _dbContext.SpGetDriverAssignedStoresBySystemOwner.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (DriverStores != null)
                {
                    return (true, DriverStores, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverStoresAsync(Guid DriverId, Guid StoreId, bool IsActive)
        {
            try
            {
                string sqlquery = "Exec SpAssignUnAssignDrivers @DriverId,@StoreId,@IsActive";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@DriverId", Value = DriverId },
                     new SqlParameter { ParameterName = "@StoreId", Value = StoreId },
                     new SqlParameter { ParameterName = "@IsActive", Value = IsActive }
                };

                var DriverStores = await _dbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }

}
