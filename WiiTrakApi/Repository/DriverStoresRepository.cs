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

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverStoresAsync(DriverStoreDetailsDto DriverStoreDetailsDto)
        {
            try
            {
                string sqlquery = "Exec SpAssignUnAssignDrivers @DriverId,@StoreId,@IsActive,@AssignedBy";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@DriverId", Value = DriverStoreDetailsDto.DriverId },
                     new SqlParameter { ParameterName = "@StoreId", Value =  DriverStoreDetailsDto.Id },
                     new SqlParameter { ParameterName = "@IsActive", Value = DriverStoreDetailsDto.DriverStoresIsActive },
                     new SqlParameter { ParameterName = "@AssignedBy", Value = DriverStoreDetailsDto.AssignedBy }
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
