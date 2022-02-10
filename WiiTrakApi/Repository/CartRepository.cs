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
        private readonly ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, CartModel? Cart, string? ErrorMessage)> GetCartByIdAsync(Guid id)
        {
            var cart = await _dbContext.Carts
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
                var carts = await _dbContext.Carts
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
                var deliveryTicket = await _dbContext.DeliveryTickets
                    .Where(x => x.Id == deliveryTicketId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                var cartHistory = await _dbContext.CartHistory
                    .Where(x => x.DeliveryTicketId == deliveryTicketId)
                    .AsNoTracking()
                    .ToListAsync();
                var carts = new List<CartModel>();
                foreach(var cart in cartHistory)
                {
                    var _cart = await _dbContext.Carts
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

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByStoreIdAsync(Guid storeId)
        {
            try
            {
                var carts = await _dbContext.Carts
                    .Include(x => x.Store)
                    .Include(x => x.TrackingDevice)
                    .Where(x => x.StoreId == storeId && (x.Status == CartStatus.OutsideGeofence || x.Status == CartStatus.PickedUp))
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

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDriverIdAsync(Guid driverId)
        {
            try
            {
                var driverStores = await _dbContext.DriverStores
                    .Where(x => x.DriverId == driverId)
                    .AsNoTracking()
                    .ToListAsync();

                var cartList = new List<CartModel>();

                foreach (var driverStore in driverStores)
                {
                    var carts = await _dbContext.Carts
                        .Where(x => x.StoreId == driverStore.StoreId && (x.Status == CartStatus.OutsideGeofence || x.Status == CartStatus.PickedUp))
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

        public async Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCorporateIdAsync(Guid corporateId)
        {
            try
            {
                var corporateStores = await _dbContext.Stores.Where(x => x.CorporateId == corporateId).ToListAsync();

                var cartList = new List<CartModel>();

                foreach (var store in corporateStores)
                {
                    var carts = await _dbContext.Carts
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
                var companyStores = await _dbContext.Stores.Where(x => x.CompanyId == companyId).ToListAsync();

                var cartList = new List<CartModel>();

                foreach (var store in companyStores)
                {
                    var carts = await _dbContext.Carts
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
                var carts = await _dbContext.Carts
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
                var exists = await _dbContext.Carts.AnyAsync(x => x.Id.Equals(id));
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
                await _dbContext.Carts.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
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
                _dbContext.Carts.Update(cart);
                await _dbContext.SaveChangesAsync();
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
                var recordToDelete = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Cart not found");
                _dbContext.Carts.Remove(recordToDelete);
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
