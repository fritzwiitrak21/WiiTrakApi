using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class CorporateRepository : ICorporateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CorporateRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, CorporateModel? Corporate, string? ErrorMessage)> GetCorporateByIdAsync(Guid id)
        {
            var company = await _dbContext.Corporates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (company is not null)
            {
                return (true, (CorporateModel)company, null);
            }
            return (false, null, "No corporate found");
        }

        public async Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetAllCorporatesAsync()
        {
            try
            {
                var corporations = await _dbContext.Corporates
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (corporations.Any())
                {
                    return (true, corporations, null);
                }
                return (false, null, "No corporates found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesByConditionAsync(Expression<Func<CorporateModel, bool>> expression)
        {
            try
            {
                var corporations = await _dbContext.Corporates
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (corporations.Any())
                {
                    return (true, corporations, null);
                }
                return (false, null, "No corporates found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesByCompanyIdAsync(Guid companyId)
        {
            try
            {
                //var companyCorporates = await _dbContext.CompanyCorporates
                //    .Where(x => x.CompanyId == companyId)
                //    .AsNoTracking()
                //    .ToListAsync();

                //var corporations = companyCorporates.Select(x => x.Corporate).ToList();

                string sqlquery = "Exec SpGetCorporatesByCompanyId @CompanyId";

                List<SqlParameter> parms;


                parms = new List<SqlParameter>
                {
                    
                     new SqlParameter { ParameterName = "@CompanyId", Value = companyId },
                    
                };

                var corporates = await _dbContext.Corporates.FromSqlRaw<CorporateModel>(sqlquery, parms.ToArray()).ToListAsync();



                if (corporates.Any())
                {
                    return (true, corporates, null);
                }
                return (false, null, "No corporates found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesBySystemOwnerIdAsync(Guid SystemOwnerId)
        {
            try
            {
                string sqlquery = "Exec SpGetCorporatesBySystemOwnerId @SystemOwnerId";

                List<SqlParameter> parms;
                parms = new List<SqlParameter>
                {

                     new SqlParameter { ParameterName = "@SystemOwnerId", Value = SystemOwnerId },

                };

                var corporates = await _dbContext.Corporates.FromSqlRaw<CorporateModel>(sqlquery, parms.ToArray()).ToListAsync();

                if (corporates.Any())
                {
                    return (true, corporates, null);
                }
                return (false, null, "No corporates found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, CorporateReportDto? Report, string? ErrorMessage)> GetCorporateReportById(Guid id)
        {
            try
            {
                var report = new CorporateReportDto();

                var corporateStores = await _dbContext.Stores.Where(x => x.CorporateId == id).ToListAsync();


                var carts = new List<CartModel>();

                foreach (var store in corporateStores)
                {
                    var storeCarts = await _dbContext.Stores
                        .Include(x => x.Carts)
                        .FirstOrDefaultAsync(x => x.Id == store.Id);
                    carts.AddRange(storeCarts.Carts);
                }

                int totalStores = corporateStores.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);

                int cartsOnVehicleToday = 0;
                int cartsDeliveredToday = 0;
                int cartsNeedingRepairToday = 0;
                int cartsLostToday = 0;


                cartsOnVehicleToday = carts.Count(x => x.UpdatedAt is not null &&
                                                    x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                    x.Status == CartStatus.PickedUp);

                cartsDeliveredToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Status == CartStatus.InsideGeofence);

                cartsNeedingRepairToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Condition == CartCondition.Damage);

                cartsLostToday = carts.Count(x => x.UpdatedAt is not null &&
                                                  x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                  x.Status == CartStatus.Lost);

                report.CorporateId = id;

                report.TotalStores = totalStores;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsNeedingRepair = totalCartsNeedingRepair;
                report.TotalCartsOutsideStore = totalCartsOutsideStore;

                report.CartsDeliveredToday = cartsDeliveredToday;
                report.CartsLostToday = cartsLostToday;
                report.CartsNeedingRepairToday = cartsNeedingRepairToday;
                report.CartsOnVehicleToday = cartsOnVehicleToday;

                return (true, report, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CorporateExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Corporates.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCorporateAsync(CorporateModel corporate)
        {
            try
            {
                await _dbContext.Corporates.AddAsync(corporate);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCorporateAsync(CorporateModel corporate)
        {
            try
            {
                _dbContext.Corporates.Update(corporate);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteCorporateAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Corporates.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Corporate not found");
                _dbContext.Corporates.Remove(recordToDelete);
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
