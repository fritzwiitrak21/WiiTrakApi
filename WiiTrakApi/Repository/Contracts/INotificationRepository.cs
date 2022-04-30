using WiiTrakApi.Models;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Repository.Contracts
{
    public interface INotificationRepository
    {
        Task<(bool IsSuccess, List<NotificationModel>? Notification, string? ErrorMessage)> GetAllNotificationsAsync();
        Task<(bool IsSuccess, List<SpGetNotification>? Notification, string? ErrorMessage)> GetNotificationAsync(Guid Id);
        Task<(bool IsSuccess, string? ErrorMessage)> AddNewNotificationAsync(NotificationModel Notification);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateNotifiedTimeAsync(Guid Id);
    }
}
