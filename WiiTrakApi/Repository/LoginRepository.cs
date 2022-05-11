using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using WiiTrakApi.Services;
using WiiTrakApi.Controllers;

namespace WiiTrakApi.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LoginRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, List<UsersModel>? Users, string? ErrorMessage)> GetAllUserDetailsAsync()
        {
            try
            {

                var Users = await _dbContext.Users
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

                var User = await _dbContext.Users
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
                //var Users = await _dbContext.Users
                //   .Where(x => x.Email == login.Username && x.Password == EnPassword && x.IsActive == true)
                //   .AsNoTracking().ToListAsync();

                var Users = await _dbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == Username && x.Password == EnPassword && x.IsActive == true);



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


                var Users = await _dbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == forgot.Username && x.IsActive == true);

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
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

