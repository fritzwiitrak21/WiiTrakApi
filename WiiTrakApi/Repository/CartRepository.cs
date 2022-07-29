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


namespace WiiTrakApi.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext DbContext;
        const string ErrorMessage = "No carts found";
        public CartRepository(ApplicationDbContext DBContext)
        {
            DbContext = DBContext;
        }

        public async Task<(bool IsSuccess, CartModel? Cart, string? ErrorMessage)> GetCartByIdAsync(Guid id)
        {
            var cart = await DbContext.Carts
                .Include(x => x.TrackingDevice)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (cart is not null)
            {
                return (true, cart, null);
            }
            return (false, null, ErrorMessage);
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
                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDeliveryTicketIdAsync(Guid DeliveryTicketId)
        {
            try
            {
                var cartHistory = await DbContext.CartHistory
                    .Where(x => x.DeliveryTicketId == DeliveryTicketId)
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
                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
     
        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByStoreIdAsync(Guid StoreId)
        {
            try
            {
                var carts = await DbContext.Carts
                    .Include(x => x.Store)
                    .Where(x => x.StoreId == StoreId) 
                    .AsNoTracking().OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                if (carts.Any())
                {
                    return (true, carts, null);
                }

                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDriverIdAsync(Guid DriverId)
        {
            try
            {
                var driverStores = await DbContext.DriverStores
                    .Where(x => x.DriverId == DriverId && x.IsActive)
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
                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCorporateIdAsync(Guid CorporateId)
        {
            try
            {
                var corporateStores = await DbContext.Stores.Where(x => x.CorporateId == CorporateId).ToListAsync();
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
                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCompanyIdAsync(Guid CompanyId)
        {
            try
            {
                var companyStores = await DbContext.Stores.Where(x => x.CompanyId == CompanyId).ToListAsync();
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
                return (false, null, ErrorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByTechnicianIdAsync(Guid TechnicianId)
        {
            try
            {
                var technician=await DbContext.Technicians.Where(x=>x.Id == TechnicianId).FirstOrDefaultAsync();
                var company = await DbContext.Companies.Where(x => x.SystemOwnerId == technician.SystemOwnerId).ToListAsync();
                var companyStores = new List<StoreModel>();
                if (company != null)
                {
                    foreach (var com in company)
                    {
                        var store = await DbContext.Stores.Where(x => x.CompanyId == com.Id && x.IsConnectedStore).ToListAsync();
                        companyStores.AddRange(store);
                    }

                }
                var cartList = new List<CartModel>();
                foreach (var store in companyStores)
                {
                    var carts = await DbContext.Carts
                        .Where(x => x.StoreId == store.Id && store.IsConnectedStore)
                        .Include(x => x.Store)
                        .Include(x => x.TrackingDevice)
                        .AsNoTracking()
                        .ToListAsync();
                    cartList.AddRange(carts);
                }
                if (cartList.Any())
                {
                    foreach(var cart in cartList)
                    {
                        var device = await DbContext.Devices.Where(x => x.Id == cart.DeviceId).FirstOrDefaultAsync();
                        if (device != null)
                        {
                            cart.Device = device;
                        }
                    }
                    return (true, cartList, null);
                }
                return (false, null, ErrorMessage);
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
                return (false, null, ErrorMessage);
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
        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartStatusByIdAsync(CartModel StoreCart)
        {
            try
            {
                StoreCart.Status = Enums.CartStatus.OutsideGeofence;
                StoreCart.UpdatedAt = DateTime.UtcNow;

                var NewCartHistry = new CartHistoryModel
                {
                    StoreId = StoreCart.StoreId,
                    Status = Enums.CartStatus.OutsideGeofence,
                    CartId = StoreCart.Id,
                    DeviceId = StoreCart.DeviceId,
                    Condition = StoreCart.Condition,
                    IsDelivered = false,
                    IssueType = StoreCart.IssueType,
                    IssueDescription = StoreCart.IssueDescription,
                    CreatedAt = DateTime.UtcNow,
                };

                DbContext.Carts.Update(StoreCart);
                await DbContext.CartHistory.AddAsync(NewCartHistry);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch(Exception ex)
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
