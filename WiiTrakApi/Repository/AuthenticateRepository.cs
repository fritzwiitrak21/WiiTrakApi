/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Cores;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace WiiTrakApi.Repository
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;


        public AuthenticateRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }


        public JwtSecurityToken Login(LoginDto login)
        {
            try
            {
                var result = GetUsersDetailsByLoginAsync(login);


                if (result.Result.Users is not null)
                {
                    var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, "WiiTrak"),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                    authClaims.Add(new Claim(ClaimTypes.Role, "admin"));

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(12),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return token;
                }
                return default;
            }
            catch (Exception ex)
            {

                return default;
            }
        }

        public async Task<(bool IsSuccess, UsersModel? Users, string? ErrorMessage)> GetUsersDetailsByLoginAsync(LoginDto login)
        {
            try
            {

                var Username = Core.Base64Decrypt(login.Username);
                var Password = Core.Base64Decrypt(login.Password);

                var EnPassword = Core.EncryptText(Password);
                
                var Users = await _dbContext.Users
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
    }
}
