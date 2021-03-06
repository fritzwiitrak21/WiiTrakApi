/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.SPModels;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, List<NotificationModel>? Notification, string? ErrorMessage)> GetAllNotificationsAsync()
        {
            var notifications = await _dbContext.Notification
                                .AsNoTracking()
                                .ToListAsync();

            if (notifications is not null)
            {
                return (true, notifications, null);
            }
            return (false, null, "No notifications found");
        }

        public async Task<(bool IsSuccess, List<SpGetNotification>? Notification, string? ErrorMessage)> GetNotificationAsync(Guid Id)
        {
            const string sqlquery = "Exec SpGetNotification @Id";

            List<SqlParameter> parms;

            parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = Id },
                };
            
            var notifications = await _dbContext.SpGetNotifications.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

            if (notifications is not null)
            {
                return (true, notifications, null);
            }
            return (false, null, "No notifications found");
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> AddNewNotificationAsync(NotificationModel Notification)
        {
            try
            {
                await _dbContext.Notification.AddAsync(Notification);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateNotifiedTimeAsync(Guid Id)
        {
            try
            {
                const string sqlquery = "Exec SpUpdateNotifiedTime @Id";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = Id }
                };

                await _dbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
