/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using Microsoft.Data.SqlClient;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext DbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<(bool IsSuccess, CartModel? Cart, string? ErrorMessage)> GetCartByIdAsync(Guid id)
        {
            var cart = await DbContext.Carts
                .Include(x => x.TrackingDevice)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (cart is not null)
            {
                return (true, (CartModel)cart, null);
            }
            return (false, null, "No cart found");
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetAllCartsAsync()
        {
            try
            {
                var carts = await DbContext.Carts
                    .Include(x => x.TrackingDevice)
                    .Include(x => x.Store)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();
                if (carts.Any())
                {
                    return (true, carts, null);
                }
                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDeliveryTicketIdAsync(Guid deliveryTicketId)
        {
            try
            {
                var deliveryTicket = await DbContext.DeliveryTickets
                    .Where(x => x.Id == deliveryTicketId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                var cartHistory = await DbContext.CartHistory
                    .Where(x => x.DeliveryTicketId == deliveryTicketId)
                    .AsNoTracking()
                    .ToListAsync();
                var carts = new List<CartModel>();
                foreach(var cart in cartHistory)
                {
                    var _cart = await DbContext.Carts
                        .Include(x => x.Store)
                        .Include(x => x.TrackingDevice)
                        .Where(x => x.Id == cart.CartId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                    if (_cart != null)
                    {
                        carts.Add(_cart);
                    }
                }
                if (carts.Any())
                {
                    return (true, carts, null);
                }
                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<SPGetCartsDetailsByDeliveryTicketId>? Carts, string? ErrorMessage)> GetCartHistoryByDeliveryTicketIdAsync(Guid deliveryTicketId)
        {
            try
            {
                List<SqlParameter> parms;
                const string sqlquery = "Exec SPGetCartsDetailsByDeliveryTicketId @DeliveryTicketId";
                parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@DeliveryTicketId", Value =deliveryTicketId  }

                };

                var Carts = await DbContext.SPGetCartsDetailsByDeliveryTicketId.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

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
        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByStoreIdAsync(Guid storeId)
        {
            try
            {
                var carts = await DbContext.Carts
                    .Include(x => x.Store)
                    //.Include(x => x.TrackingDevice)
                    .Where(x => x.StoreId == storeId) 
                    //&& (x.Status == CartStatus.OutsideGeofence || x.Status == CartStatus.PickedUp))
                    .AsNoTracking().OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                if (carts.Any())
                {
                    return (true, carts, null);
                }

                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDriverIdAsync(Guid driverId)
        {
            try
            {
                var driverStores = await DbContext.DriverStores
                    .Where(x => x.DriverId == driverId && x.IsActive)
                    .AsNoTracking()
                    .ToListAsync();
                var cartList = new List<CartModel>();

                foreach (var driverStore in driverStores)
                {
                    var carts = await DbContext.Carts
                        .Where(x => x.StoreId == driverStore.StoreId && x.IsActive && (x.Status != CartStatus.InsideGeofence))
                        .Include(x => x.Store)
                        .Where(x=>x.Store.IsActive)
                        .Include(x => x.TrackingDevice)
                        .Where(x => x.TrackingDevice.IsActive)
                        .AsNoTracking()
                        .ToListAsync();

                    cartList.AddRange(carts);
                }
                if (cartList.Any())
                {
                    return (true, cartList, null);
                }
                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCorporateIdAsync(Guid corporateId)
        {
            try
            {
                var corporateStores = await DbContext.Stores.Where(x => x.CorporateId == corporateId).ToListAsync();
                var cartList = new List<CartModel>();
                foreach (var store in corporateStores)
                {
                    var carts = await DbContext.Carts
                        .Where(x => x.StoreId == store.Id && (x.Status == CartStatus.OutsideGeofence || x.Status == CartStatus.PickedUp))
                        .Include(x => x.Store)
                        .Include(x => x.TrackingDevice)
                        .AsNoTracking()
                        .ToListAsync();
                    cartList.AddRange(carts);
                }
                if (cartList.Any())
                {
                    return (true, cartList, null);
                }
                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCompanyIdAsync(Guid companyId)
        {
            try
            {
                var companyStores = await DbContext.Stores.Where(x => x.CompanyId == companyId).ToListAsync();
                var cartList = new List<CartModel>();
                foreach (var store in companyStores)
                {
                    var carts = await DbContext.Carts
                        .Where(x => x.StoreId == store.Id && (x.Status == CartStatus.OutsideGeofence || x.Status == CartStatus.PickedUp))
                        .Include(x => x.Store)
                        .Include(x => x.TrackingDevice)
                        .AsNoTracking()
                        .ToListAsync();
                    cartList.AddRange(carts);
                }
                if (cartList.Any())
                {
                    return (true, cartList, null);
                }
                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByConditionAsync(Expression<Func<CartModel, bool>> expression)
        {
            try
            {
                var carts = await DbContext.Carts
                    .Include(x => x.TrackingDevice)
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();
                if (carts.Any())
                {
                    return (true, carts, null);
                }
                return (false, null, "No carts found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CartExistsAsync(Guid id)
        {
            try
            {
                var exists = await DbContext.Carts.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCartAsync(CartModel cart)
        {
            try
            {
                await DbContext.Carts.AddAsync(cart);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartAsync(CartModel cart)
        {
            try
            {
               
                DbContext.Carts.Update(cart);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartAsync(Guid id)
        {
            try
            {
                var recordToDelete = await DbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Cart not found");
                }
                DbContext.Carts.Remove(recordToDelete);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await DbContext.SaveChangesAsync() >= 0;
        }
    }
}
