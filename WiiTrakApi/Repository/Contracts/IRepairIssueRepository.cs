using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IRepairIssueRepository
    {
        Task<(bool IsSuccess, RepairIssueModel? RepairIssue, string? ErrorMessage)> GetRepairIssueByIdAsync(Guid id);

        Task<(bool IsSuccess, List<RepairIssueModel>? RepairIssues, string? ErrorMessage)> GetAllRepairIssuesAsync();

        Task<(bool IsSuccess, string? ErrorMessage)> CreateRepairIssueAsync(RepairIssueModel repairIssue);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateRepairIssueAsync(RepairIssueModel repairIssue);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteRepairIssueAsync(Guid id);
    }
}
