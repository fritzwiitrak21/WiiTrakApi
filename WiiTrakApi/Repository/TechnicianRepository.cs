/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using WiiTrakApi.Cores;
using WiiTrakApi.Data;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class TechnicianRepository: ITechnicianRepository
    {
        private readonly ApplicationDbContext DbContext;

        public TechnicianRepository(ApplicationDbContext DBContext)
        {
            DbContext = DBContext;
        }

        public async Task<(bool IsSuccess, TechnicianModel? Technician, string? ErrorMessage)> GetTechnicianByIdAsync(Guid id)
        {
            var technician = await DbContext.Technicians
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (technician is not null)
            {
                return (true, technician, null);
            }
            return (false, null, "No technician found");
        }

        public async Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetAllTechniciansAsync()
        {
            try
            {
                var technicians = await DbContext.Technicians
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (technicians.Any())
                {
                    return (true, technicians, null);
                }
                return (false, null, "No technicians found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansByConditionAsync(Expression<Func<TechnicianModel, bool>> expression)
        {
            try
            {
                var technicians = await DbContext.Technicians
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (technicians.Any())
                {
                    return (true, technicians, null);
                }
                return (false, null, "No technicians found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansBySystemOwnerIdAsync(Guid SystemOwnerId)
        {
            try
            {
                var technicians = await DbContext.Technicians
                    .Where(x => x.SystemOwnerId == SystemOwnerId)
                    .AsNoTracking()
                    .ToListAsync();

                if (technicians.Any())
                {
                    return (true, technicians, null);
                }
                return (false, null, "No technicians found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansByCompanyIdAsync(Guid CompanyId)
        {
            try
            {
                var technicians = await DbContext.Technicians
                    .Where(x => x.CompanyId == CompanyId)
                    .AsNoTracking()
                    .ToListAsync();

                if (technicians.Any())
                {
                    return (true, technicians, null);
                }
                return (false, null, "No technicians found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> TechnicianExistsAsync(Guid id)
        {
            try
            {
                var exists = await DbContext.Technicians.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateTechnicianAsync(TechnicianModel technician, int RoleId)
        {
            try
            {
                if (RoleId == 3)
                {
                    var CompanyDetails = await DbContext.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == technician.CompanyId);
                    technician.SystemOwnerId = CompanyDetails.SystemOwnerId;
                }
                await DbContext.Technicians.AddAsync(technician);

                #region Adding Technician details to users table
                UsersModel user = new UsersModel();
                user.Id = technician.Id;
                user.FirstName = technician.FirstName;
                user.LastName = technician.LastName;
                user.Password = Core.CreatePassword();
                user.Email = technician.Email;
                user.AssignedRole = (int)Role.Technician;
                user.CreatedAt =
                user.PasswordLastUpdatedAt = DateTime.UtcNow;
                user.IsActive = true;
                user.IsFirstLogin = true;

                await DbContext.Users.AddAsync(user);
                #endregion

                await DbContext.SaveChangesAsync();
                return (true, null);
            }
             catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateTechnicianAsync(TechnicianModel technician, int RoleId)
        {
            try
            {
                if (RoleId == 3)
                {
                    var CompanyDetails = await DbContext.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == technician.CompanyId);
                    technician.SystemOwnerId = CompanyDetails.SystemOwnerId;
                }
                DbContext.Technicians.Update(technician);
                #region Update Technician details to users table

                const string sqlquery = "Exec SpUpdateUserDetails @Id,@FirstName,@LastName,@IsActive,@Email";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = technician.Id},
                     new SqlParameter { ParameterName = "@FirstName", Value = technician.FirstName },
                     new SqlParameter { ParameterName = "@LastName", Value = technician.LastName },
                     new SqlParameter { ParameterName = "@IsActive", Value = true },
                     new SqlParameter { ParameterName = "@Email", Value = technician.Email }
                };

                var Result = await DbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());
                #endregion
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteTechnicianAsync(Guid id)
        {
            try
            {
                var recordToDelete = await DbContext.Technicians.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Technician not found");
                }
                DbContext.Technicians.Remove(recordToDelete);
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
