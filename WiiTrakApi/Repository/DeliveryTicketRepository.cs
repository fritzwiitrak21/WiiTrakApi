using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class DeliveryTicketRepository : IDeliveryTicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeliveryTicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, DeliveryTicketModel? DeliveryTicket, string? ErrorMessage)> GetDeliveryTicketByIdAsync(Guid id)
        {
            var deliveryTicket = await _dbContext.DeliveryTickets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (deliveryTicket is not null)
            {
                return (true, deliveryTicket, null);
            }
            return (false, null, "No delivery ticket found");
        }

        public async Task<(bool IsSuccess, DeliveryTicketSummaryDto? DeliveryTicketSummary, string? ErrorMessage)> GetDeliveryTicketSummaryByIdAsync(Guid id)
        {
            var deliveryTicket = await _dbContext.DeliveryTickets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);


            var cartHistory = await _dbContext.CartHistory
                .Where(x => x.DeliveryTicketId == id)
                .AsNoTracking()
                .ToListAsync();

            var summary = new DeliveryTicketSummaryDto
            {
                Delivered = cartHistory.Count(x => x.IsDelivered),
                Lost = cartHistory.Count(x => x.Status == CartStatus.Lost),
                Damaged = cartHistory.Count(x => x.Condition == CartCondition.Damage),
                Trashed = cartHistory.Count(x => x.Condition == CartCondition.DamageBeyondRepair)
            };

            if (summary is not null)
            {
                return (true, summary, null);
            }
            return (false, null, "No delivery ticket found");
        }

        public async Task<(bool IsSuccess, long DeliveryTicketNumber, string? ErrorMessage)> GetDeliveryTicketNumberAsync(Guid serviceProviderId)
        {
            bool anyExists = await _dbContext.DeliveryTickets.AnyAsync(x => x.ServiceProviderId == serviceProviderId);
            if (!anyExists)
            {
                return (true, 1, null);
            }

            var maxTicketNumber = await _dbContext.DeliveryTickets
                .MaxAsync(x => x.DeliveryTicketNumber);

            return (true, (maxTicketNumber + 1), null);
        }


        public async Task<(bool IsSuccess, List<DeliveryTicketModel>? DeliveryTickets, string? ErrorMessage)> GetAllDeliveryTicketsAsync()
        {
            try
            {
                var deliveryTickets = await _dbContext.DeliveryTickets
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (deliveryTickets.Any())
                {
                    return (true, deliveryTickets, null);
                }
                return (false, null, "No delivery tickets found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<DeliveryTicketModel>? DeliveryTickets, string? ErrorMessage)> GetDeliveryTicketsByConditionAsync(Expression<Func<DeliveryTicketModel, bool>> expression)
        {
            try
            {
                var deliveryTickets = await _dbContext.DeliveryTickets
                    .Where(expression)
                    .Select(x => x).Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

                if (deliveryTickets.Any())
                {
                    return (true, deliveryTickets, null);
                }
                return (false, null, "No delivery tickets found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<DeliveryTicketModel>? DeliveryTickets, string? ErrorMessage)> GetDeliveryTicketsByPrimaryIdAsync(Guid Id, Enums.Role role)
        {
            try
            {
                List<DeliveryTicketModel> list = new List<DeliveryTicketModel>();
                string sqlquery = "Exec SpGetDeliveryTickets @Id,@Role";
                List<SqlParameter> parms;


                parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value =Id  },
                    new SqlParameter { ParameterName = "@Role", Value =(int)role }

                };

                list = await _dbContext.DeliveryTickets.FromSqlRaw<DeliveryTicketModel>(sqlquery, parms.ToArray()).ToListAsync();



                if (list.Any())
                {
                    return (true, list, null);
                }
                return (false, null, "No delivery  tickets found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateDeliveryTicketAsync(DeliveryTicketModel deliveryTicket)
        {
            try
            {
                var storedetails = await _dbContext.Stores
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == deliveryTicket.StoreId);
                deliveryTicket.SignOffRequired = storedetails.IsSignatureRequired;
                deliveryTicket.IsActive = true;

                await _dbContext.DeliveryTickets.AddAsync(deliveryTicket);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateDeliveryTicketAsync(DeliveryTicketModel deliveryTicket)
        {
            try
            {
                
                var storedetails = await _dbContext.Stores
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == deliveryTicket.StoreId);
                deliveryTicket.SignOffRequired = storedetails.IsSignatureRequired;
                deliveryTicket.IsActive = true; 
                _dbContext.DeliveryTickets.Update(deliveryTicket);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteDeliveryTicketAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.DeliveryTickets.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Delivery ticket not found");
                _dbContext.DeliveryTickets.Remove(recordToDelete);
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
