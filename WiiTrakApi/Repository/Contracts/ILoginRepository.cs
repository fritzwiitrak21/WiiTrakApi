using WiiTrakApi.Models;
using WiiTrakApi.DTOs;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ILoginRepository
    {
        Task<(bool IsSuccess, List<UsersModel>? Users, string? ErrorMessage)> GetAllUserDetailsAsync();
        Task<(bool IsSuccess, UsersModel? User, string? ErrorMessage)> GetUserDetailByIdAsync(Guid Id);
        Task<(bool IsSuccess, UsersModel? Users, string? ErrorMessage)> GetUsersDetailsByLoginAsync(LoginDto login);
        Task<(bool IsSuccess, UsersModel? Users, string? ErrorMessage)> GetUsersDetailsByUserNameAsync(ForgotPasswordDto forgot);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateUserPasswordAsync(UsersModel user);
    }
}
