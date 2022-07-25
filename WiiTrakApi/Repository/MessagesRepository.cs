/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using Microsoft.Data.SqlClient;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Repository
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ApplicationDbContext DbContext;

        public MessagesRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<(bool IsSuccess, List<MessagesModel>? Messages, string? ErrorMessage)> GetAllMessagesAsync()
        {
            try
            {
                var Messages = await DbContext.Messages
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (Messages.Any())
                {
                    return (true, Messages, null);
                }
                
                return (false, null, "No message found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, MessagesModel? Message, string? ErrorMessage)> GetMessageAsync(Guid Id)
        {
            try
            {
                var Message = await DbContext.Messages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (Message is not null)
                {
                    return (true, Message, null);
                }
                return (false, null, "No message found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<SpGetMessagesById>? Messages, string? ErrorMessage)> GetMessagesBIdAsync(Guid Id, int RoleId)
        {
            try
            {
                const string sqlquery = "Exec SpGetMessagesById @Id,@RoleId";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = Id },
                     new SqlParameter { ParameterName = "@RoleId", Value = RoleId },
                };

                var Messages = await DbContext.SpGetMessagesById.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();


                if (Messages.Any())
                {
                    return (true, Messages, null);
                }
                return (false, null, "No messages found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> AddNewMessageAsync(MessagesModel NewMessage)
        {
            try
            {
                await DbContext.Messages.AddAsync(NewMessage);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessageAsync(MessagesModel Message)
        {
            try
            {

                DbContext.Messages.Update(Message);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessageDeliveredTimeAsync(Guid Id)
        {
            try
            {
                const string sqlquery = "Exec SpUpdateMessagesDeliveredTime @Id";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = Id }
                };

                await DbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessageActionAsync(Guid Id, string ActionTaken)
        {
            try
            {
                const string sqlquery = "Exec SpUpdateMessageAction @Id,@ActionTaken";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = Id },
                     new SqlParameter { ParameterName = "@ActionTaken", Value = ActionTaken }
                };

                await DbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
