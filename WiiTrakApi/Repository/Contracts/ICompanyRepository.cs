using System.Linq.Expressions;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICompanyRepository
    {
        Task<(bool IsSuccess, CompanyModel? Company, string? ErrorMessage)> GetCompanyByIdAsync(Guid id);

        Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetAllCompaniesAsync();

        Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetCompaniesByConditionAsync(Expression<Func<CompanyModel, bool>> expression);
        Task<(bool IsSuccess, CompanyModel? Company, string? ErrorMessage)> GetParentCompanyAsync(Guid subcompanyId);
        Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetPrimaryCompaniesByCorporateIdAsync(Guid corporateId);
        Task<(bool IsSuccess, List<CompanyModel>? Companies, string? ErrorMessage)> GetCompaniesBySystemOwnerId(Guid systemownerId);

        Task<(bool IsSuccess, CompanyReportDto? Report, string? ErrorMessage)> GetCompanyReportById(Guid id);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CompanyExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateCompanyAsync(CompanyModel company);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCompanyAsync(CompanyModel company);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteCompanyAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
