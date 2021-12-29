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

        Task<(bool IsSuccess, string? ErrorMessage)> CreateTechnicianAsync(TechnicianModel technician);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateTechnicianAsync(TechnicianModel technician);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteTechnicianAsync(Guid id);
    }
}
