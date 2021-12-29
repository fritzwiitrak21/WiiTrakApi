using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IDriverRepository
    {
        Task<(bool IsSuccess, DriverModel? Driver, string? ErrorMessage)> GetDriverByIdAsync(Guid id);

        Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetAllDriversAsync();

        Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetDriversByConditionAsync(Expression<Func<DriverModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> DriverExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateDriverAsync(DriverModel driver);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverAsync(DriverModel driver);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteDriverAsync(Guid id);
    }
}
