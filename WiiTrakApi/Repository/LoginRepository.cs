﻿/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;

namespace WiiTrakApi.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext DbContext;

        public LoginRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<(bool IsSuccess, List<UsersModel>? Users, string? ErrorMessage)> GetAllUserDetailsAsync()
        {
            try
            {
                var Users = await DbContext.Users
                            .Select(x => x)
                            .AsNoTracking().ToListAsync();

                if (Users.Any())
                {
                    return (true, Users, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, UsersModel? User, string? ErrorMessage)> GetUserDetailByIdAsync(Guid Id)
        {
            try
            {
                var User = await DbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == Id);

                if (User != null)
                {
                    return (true, User, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, UsersModel? Users, string? ErrorMessage)> GetUsersDetailsByLoginAsync(LoginDto login)
        {
            try
            {

                var Username = Core.Base64Decrypt(login.Username);
                var Password = Core.Base64Decrypt(login.Password);
                var EnPassword = Core.EncryptText(Password);

                var Users = await DbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == Username && x.Password == EnPassword && x.IsActive );

                if (Users != null)
                {
                    return (true, Users, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, UsersModel? Users, string? ErrorMessage)> GetUsersDetailsByUserNameAsync(ForgotPasswordDto forgot)
        {
            try
            {
                var Users = await DbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == forgot.Username && x.IsActive);

                if (Users != null)
                {
                    return (true, Users, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateUserPasswordAsync(UsersModel user)
        {
            try
            {
                DbContext.Users.Update(user);
                await DbContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

