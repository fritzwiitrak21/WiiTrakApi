﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, CompanyModel? Company, string? ErrorMessage)> GetCompanyByIdAsync(Guid id)
        {
            var company = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (company is not null)
            {
                return (true, (CompanyModel)company, null);
            }
            return (false, null, "No company found");
        }

        public async Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetAllCompaniesAsync()
        {
            try
            {
                var companies = await _dbContext.Companies
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (companies.Any())
                {
                    return (true, companies, null);
                }
                return (false, null, "No companies found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetCompaniesByConditionAsync(Expression<Func<CompanyModel, bool>> expression)
        {
            try
            {
                var companies = await _dbContext.Companies
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (companies.Any())
                {
                    return (true, companies, null);
                }
                return (false, null, "No companies found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetPrimaryCompaniesByCorporateIdAsync(Guid corporateId)
        {
            try
            {
                var companyCorporates = await _dbContext.CompanyCorporates
                    .Where(x => x.CorporateId == corporateId)
                    .AsNoTracking()
                    .ToListAsync();

                var companies = companyCorporates.Select(x => x.Company).ToList();

                if (companies.Any())
                {
                    return (true, companies, null);
                }
                return (false, null, "No companies found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CompanyExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Companies.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, CompanyReportDto? Report, string? ErrorMessage)> GetCompanyReportById(Guid id)
        {
            try
            {
                var report = new CompanyReportDto();

                var companyStores = await _dbContext.Stores.Where(x => x.CompanyId == id).ToListAsync();


                var carts = new List<CartModel>();

                foreach (var store in companyStores)
                {
                    var storeCarts = await _dbContext.Stores
                        .Include(x => x.Carts)
                        .FirstOrDefaultAsync(x => x.Id == store.Id);
                    carts.AddRange(storeCarts.Carts);
                }

                int totalStores = companyStores.Count();
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

                report.CompanyId = id;

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

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCompanyAsync(CompanyModel company)
        {
            try
            {
                await _dbContext.Companies.AddAsync(company);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCompanyAsync(CompanyModel company)
        {
            try
            {
                _dbContext.Companies.Update(company);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteCompanyAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Customer not found");
                _dbContext.Companies.Remove(recordToDelete);
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
