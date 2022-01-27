using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IWorkOrderRepository
    {
        Task<(bool IsSuccess, WorkOrderModel? WorkOrder, string? ErrorMessage)> GetWorkOrderByIdAsync(Guid id);

        Task<(bool IsSuccess, List<WorkOrderModel>? WorkOrders, string? ErrorMessage)> GetAllWorkOrdersAsync();

        Task<(bool IsSuccess, int WorkOrderNumber, string? ErrorMessage)> GetWorkOrderNumberAsync();
        
        Task<(bool IsSuccess, List<WorkOrderModel>? WorkOrders, string? ErrorMessage)> GetWorkOrdersByConditionAsync(Expression<Func<WorkOrderModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> WorkOrderExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateWorkOrderAsync(WorkOrderModel workOrder);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateWorkOrderAsync(WorkOrderModel workOrder);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteWorkOrderAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
