using System.Linq.Expressions;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IDeliveryTicketRepository
    {
        Task<(bool IsSuccess, DeliveryTicketModel? DeliveryTicket, string? ErrorMessage)> GetDeliveryTicketByIdAsync(Guid id);

        Task<(bool IsSuccess, DeliveryTicketSummaryDto? DeliveryTicketSummary, string? ErrorMessage)> GetDeliveryTicketSummaryByIdAsync(Guid id);

        Task<(bool IsSuccess, long DeliveryTicketNumber, string? ErrorMessage)> GetDeliveryTicketNumberAsync(Guid serviceProviderId);

        Task<(bool IsSuccess, List<DeliveryTicketModel>? DeliveryTickets, string? ErrorMessage)> GetAllDeliveryTicketsAsync();

        Task<(bool IsSuccess, List<DeliveryTicketModel>? DeliveryTickets, string? ErrorMessage)> GetDeliveryTicketsByConditionAsync(Expression<Func<DeliveryTicketModel, bool>> expression);
        Task<(bool IsSuccess, List<DeliveryTicketModel>? DeliveryTickets, string? ErrorMessage)> GetDeliveryTicketsByPrimaryIdAsync(Guid Id, Enums.Role role);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateDeliveryTicketAsync(DeliveryTicketModel deliveryTicket);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDeliveryTicketAsync(DeliveryTicketModel deliveryTicket);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteDeliveryTicketAsync(Guid id);

        Task<bool> SaveAsync();
    }
}
