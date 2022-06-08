/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
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
