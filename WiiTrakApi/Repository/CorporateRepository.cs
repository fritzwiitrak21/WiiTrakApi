/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Cores;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class CorporateRepository : ICorporateRepository
    {
        private readonly ApplicationDbContext DbContext;
        const string ErrorMessage = "No Corporate found";
        public CorporateRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<(bool IsSuccess, CorporateModel? Corporate, string? ErrorMessage)> GetCorporateByIdAsync(Guid id)
        {
            var company = await DbContext.Corporates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (company is not null)
            {
                return (true, company, null);
            }
            return (false, null, ErrorMessage);
        }
        public async Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetAllCorporatesAsync()
        {
            try
            {
                var corporations = await DbContext.Corporates
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (corporations.Any())
                {
                    return (true, corporations, null);
                }
                return (false, null, ErrorMessage);
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
                var corporations = await DbContext.Corporates
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (corporations.Any())
                {
                    return (true, corporations, null);
                }
                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesByCompanyIdAsync(Guid CompanyId)
        {
            try
            {
                //var companyCorporates = await _dbContext.CompanyCorporates
                //    .Where(x => x.CompanyId == companyId)
                //    .AsNoTracking()
                //    .ToListAsync();
                //var corporations = companyCorporates.Select(x => x.Corporate).ToList();
                const string sqlquery = "Exec SpGetCorporatesByCompanyId @CompanyId";
                List<SqlParameter> parms;
                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@CompanyId", Value = CompanyId },
                };
                var corporates = await DbContext.Corporates.FromSqlRaw<CorporateModel>(sqlquery, parms.ToArray()).ToListAsync();
                if (corporates.Any())
                {
                    return (true, corporates, null);
                }
                return (false, null, ErrorMessage);
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
                const string sqlquery = "Exec SpGetCorporatesBySystemOwnerId @SystemOwnerId";
                List<SqlParameter> parms;
                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@SystemOwnerId", Value = SystemOwnerId },
                };
                var corporates = await DbContext.Corporates.FromSqlRaw<CorporateModel>(sqlquery, parms.ToArray()).ToListAsync();
                if (corporates.Any())
                {
                    return (true, corporates, null);
                }
                return (false, null, ErrorMessage);
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

                var corporateStores = await DbContext.Stores.Where(x => x.CorporateId == id).ToListAsync();


                var carts = new List<CartModel>();

                foreach (var store in corporateStores)
                {
                    var storeCarts = await DbContext.Stores
                        .Include(x => x.Carts)
                        .FirstOrDefaultAsync(x => x.Id == store.Id);
                    carts.AddRange(storeCarts.Carts);
                }

                int totalStores = corporateStores.Count;
                int totalCarts = carts.Count;
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
                var exists = await DbContext.Corporates.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCorporateAsync(CorporateModel corporate, Guid CompanyId, int RoleId)
        {
            try
            {
                await DbContext.Corporates.AddAsync(corporate);
                #region Adding Corporate details to users table
                UsersModel user = new UsersModel();
                user.Id = corporate.Id;
                user.FirstName = corporate.Name;
                user.Password = Core.CreatePassword();
                user.Email = corporate.Email;
                user.AssignedRole = (int)Role.Corporate;
                user.CreatedAt =
                user.PasswordLastUpdatedAt = DateTime.UtcNow;
                user.IsActive = true;
                user.IsFirstLogin = true;

                await DbContext.Users.AddAsync(user);
                #endregion

                #region Adding Corporate details to CompanyCorporate table
                if (RoleId == 3 || RoleId == 4)
                {
                    CompanyCorporateModel companycorporate = new CompanyCorporateModel();
                    companycorporate.CorporateId = corporate.Id;
                    companycorporate.CompanyId = CompanyId;
                    companycorporate.CreatedAt = DateTime.UtcNow;
                    await DbContext.CompanyCorporates.AddAsync(companycorporate);
                }
                #endregion

                await DbContext.SaveChangesAsync();
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
                #region Update corporate details to users table
                const string sqlquery = "Exec SpUpdateUserDetails @Id,@FirstName,@LastName,@IsActive,@Email";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = corporate.Id},
                     new SqlParameter { ParameterName = "@FirstName", Value = corporate.Name },
                     new SqlParameter { ParameterName = "@LastName", Value = "" },
                     new SqlParameter { ParameterName = "@IsActive", Value = true },
                     new SqlParameter { ParameterName = "@Email", Value = corporate.Email }
                };
                var Result = await DbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());
                #endregion
                DbContext.Corporates.Update(corporate);
                await DbContext.SaveChangesAsync();
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
                var recordToDelete = await DbContext.Corporates.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Corporate not found");
                }
                DbContext.Corporates.Remove(recordToDelete);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<bool> SaveAsync()
        {
            return await DbContext.SaveChangesAsync() >= 0;
        }
    }
}
