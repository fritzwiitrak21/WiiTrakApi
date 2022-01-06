using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System.Linq;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class WorkOrderRepository: IWorkOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WorkOrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(bool IsSuccess, WorkOrderModel? WorkOrder, string? ErrorMessage)> GetWorkOrderByIdAsync(Guid id)
        {
            var workOrder = await _dbContext.WorkOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (workOrder is not null)
            {
                return (true, workOrder, null);
            }
            return (false, null, "No work order found");
        }

        public async Task<(bool IsSuccess, List<WorkOrderModel>? WorkOrders, string? ErrorMessage)> GetAllWorkOrdersAsync()
        {
            try
            {
                var workOrders = await _dbContext.WorkOrders
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (workOrders.Any())
                {
                    return (true, workOrders, null);
                }
                return (false, null, "No work orders found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<WorkOrderModel>? WorkOrders, string? ErrorMessage)> GetWorkOrdersByConditionAsync(Expression<Func<WorkOrderModel, bool>> expression)
        {
            try
            {
                var workOrders = await _dbContext.WorkOrders
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (workOrders.Any())
                {
                    return (true, workOrders, null);
                }
                return (false, null, "No work orders found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> WorkOrderExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.WorkOrders.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateWorkOrderAsync(WorkOrderModel workOrder)
        {
            try
            {
                await _dbContext.WorkOrders.AddAsync(workOrder);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateWorkOrderAsync(WorkOrderModel workOrder)
        {
            try
            {
                _dbContext.WorkOrders.Update(workOrder);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteWorkOrderAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.WorkOrders.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Work order not found");
                _dbContext.WorkOrders.Remove(recordToDelete);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
