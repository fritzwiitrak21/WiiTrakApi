using System.Linq.Expressions;
using WiiTrakApi.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IAuthenticateRepository
    {
        JwtSecurityToken Login(LoginDto login);
    }
}
