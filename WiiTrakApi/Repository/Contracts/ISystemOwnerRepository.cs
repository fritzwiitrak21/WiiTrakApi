using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ISystemOwnerRepository
    {
        Task<(bool IsSuccess, SystemOwnerModel? SystemOwner, string? ErrorMessage)> GetSystemOwnerByIdAsync(Guid id);

        Task<(bool IsSuccess, List<SystemOwnerModel>? SystemOwners, string? ErrorMessage)> GetAllSystemOwnersAsync();

        Task<(bool IsSuccess, List<SystemOwnerModel>? SystemOwners, string? ErrorMessage)> GetSystemOwnersByConditionAsync(Expression<Func<SystemOwnerModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> SystemOwnerExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateSystemOwnerAsync(SystemOwnerModel systemOwner);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateSystemOwnerAsync(SystemOwnerModel systemOwner);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteSystemOwnerAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
