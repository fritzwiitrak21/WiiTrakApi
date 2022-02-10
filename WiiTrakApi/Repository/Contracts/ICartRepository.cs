using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICartRepository
    {
        Task<(bool IsSuccess, CartModel? Cart, string? ErrorMessage)> GetCartByIdAsync(Guid id);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetAllCartsAsync();

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDeliveryTicketIdAsync(Guid deliveryTicketId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByStoreIdAsync(Guid storeId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDriverIdAsync(Guid driverId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCorporateIdAsync(Guid corporateId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCompanyIdAsync(Guid companyId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByConditionAsync(Expression<Func<CartModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CartExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateCartAsync(CartModel cart);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartAsync(CartModel cart);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
