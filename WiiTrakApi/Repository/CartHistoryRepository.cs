/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using Microsoft.Data.SqlClient;
using WiiTrakApi.SPModels;
namespace WiiTrakApi.Repository
{
    public class CartHistoryRepository : ICartHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartHistoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, CartHistoryModel? CartHistory, string? ErrorMessage)> GetCartHistoryByIdAsync(Guid id)
        {
            var cartHistory = await _dbContext.CartHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cartHistory is not null)
            {
                return (true, cartHistory, null);
            }
            return (false, null, "No cart history found");
        }

        public async Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetAllCartHistoryAsync()
        {
            try
            {
                var cartHistory = await _dbContext.CartHistory
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (cartHistory.Any())
                {
                    return (true, cartHistory, null);
                }
                return (false, null, "No cart history found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByCartIdAsync(Guid cartId)
        {
            try
            {
                var cartHistory = await _dbContext.CartHistory
                    .Where(x => x.CartId == cartId)
                    .AsNoTracking()
                    .ToListAsync();


                if (cartHistory.Any())
                {
                    return (true, cartHistory, null);
                }

                return (false, null, "No cart history found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByStoreIdAsync(Guid storeId)
        {
            try
            {
                var cartHistory = await _dbContext.CartHistory
                    .Where(x => x.StoreId == storeId)
                    .AsNoTracking()
                    .ToListAsync();


                if (cartHistory.Any())
                {
                    return (true, cartHistory, null);
                }

                return (false, null, "No cart history found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByServiceProviderIdAsync(Guid serviceProviderId)
        {
            try
            {
                var cartHistory = await _dbContext.CartHistory
                    .Where(x => x.ServiceProviderId == serviceProviderId)
                    .AsNoTracking()
                    .ToListAsync();


                if (cartHistory.Any())
                {
                    return (true, cartHistory, null);
                }

                return (false, null, "No cart history found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByConditionAsync(Expression<Func<CartHistoryModel, bool>> expression)
        {
            try
            {
                var cartHistory = await _dbContext.CartHistory
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (cartHistory.Any())
                {
                    return (true, cartHistory, null);
                }
                return (false, null, "No cart history found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CartHistoryExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.CartHistory.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartHistoryByDeliveryTicketIdAsync(Guid deliveryTicketId)
        {
            try
            {
                List<SqlParameter> parms;
                const string sqlquery = "Exec SPGetCartsDetailsByDeliveryTicketId @DeliveryTicketId";
                parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@DeliveryTicketId", Value =deliveryTicketId  }

                };

                var Carts = await _dbContext.Carts.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (Carts != null)
                {
                    return (true, Carts, null);
                }
                return (false, null, "No Carts");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCartHistoryAsync(CartHistoryModel cartHistory)
        {
            try
            {
                await _dbContext.CartHistory.AddAsync(cartHistory);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartHistoryAsync(CartHistoryModel cartHistory)
        {
            try
            {
                if ((_dbContext.CartHistory.Any(x => x.DeliveryTicketId == cartHistory.DeliveryTicketId && x.DriverId == cartHistory.DriverId && x.CartId==cartHistory.CartId)) && cartHistory.DeliveryTicketId!=Guid.Empty)
                {
                    var cart=_dbContext.CartHistory.Where(x => x.DeliveryTicketId == cartHistory.DeliveryTicketId && x.DriverId == cartHistory.DriverId && x.CartId == cartHistory.CartId).FirstOrDefault();
                    if (cart != null)
                    {
                        cartHistory.Id = cart.Id;
                    }
                    _dbContext.CartHistory.Update(cartHistory);
                }
                else
                {
                    await _dbContext.CartHistory.AddAsync(cartHistory);
                }
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartHistoryAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.CartHistory.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Cart history not found");
                }
                _dbContext.CartHistory.Remove(recordToDelete);
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
