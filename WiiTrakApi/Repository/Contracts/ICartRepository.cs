/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICartRepository
    {
        Task<(bool IsSuccess, CartModel? Cart, string? ErrorMessage)> GetCartByIdAsync(Guid id);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetAllCartsAsync();

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDeliveryTicketIdAsync(Guid DeliveryTicketId);
        
        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByStoreIdAsync(Guid StoreId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByDriverIdAsync(Guid DriverId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCorporateIdAsync(Guid CorporateId);

        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByCompanyIdAsync(Guid CompanyId);
        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByTechnicianIdAsync(Guid TechnicianId);
        Task<(bool IsSuccess, List<CartModel>? Carts, string? ErrorMessage)> GetCartsByConditionAsync(Expression<Func<CartModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CartExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateCartAsync(CartModel cart);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartAsync(CartModel cart);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartStatusByIdAsync(CartModel StoreCart);
        Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
