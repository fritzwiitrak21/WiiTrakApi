/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ITechnicianRepository
    {
        Task<(bool IsSuccess, TechnicianModel? Technician, string? ErrorMessage)> GetTechnicianByIdAsync(Guid id);

        Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetAllTechniciansAsync();

        Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansByConditionAsync(Expression<Func<TechnicianModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> TechnicianExistsAsync(Guid id);
        Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansBySystemOwnerIdAsync(Guid SystemOwnerId);
        Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansByCompanyIdAsync(Guid CompanyId);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateTechnicianAsync(TechnicianModel technician, int RoleId);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateTechnicianAsync(TechnicianModel technician, int RoleId);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteTechnicianAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
