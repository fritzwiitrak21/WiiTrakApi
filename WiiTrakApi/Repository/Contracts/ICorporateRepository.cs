using System.Linq.Expressions;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICorporateRepository
    {
        Task<(bool IsSuccess, CorporateModel? Corporate, string? ErrorMessage)> GetCorporateByIdAsync(Guid id);

        Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetAllCorporatesAsync();

        Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesByConditionAsync(Expression<Func<CorporateModel, bool>> expression);

        Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesByCompanyIdAsync(Guid companyId);
        Task<(bool IsSuccess, List<CorporateModel>? Corporates, string? ErrorMessage)> GetCorporatesBySystemOwnerIdAsync(Guid SystemOwnerId);

        Task<(bool IsSuccess, CorporateReportDto? Report, string? ErrorMessage)> GetCorporateReportById(Guid id);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CorporateExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateCorporateAsync(CorporateModel corporate);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCorporateAsync(CorporateModel corporate);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteCorporateAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
