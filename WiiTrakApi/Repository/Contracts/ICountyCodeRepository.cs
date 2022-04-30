using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICountyCodeRepository
    {
        Task<(bool IsSuccess, List<CountyCodeModel>? CountyList, string? ErrorMessage)> GetCountyListAsync();
    }
}
