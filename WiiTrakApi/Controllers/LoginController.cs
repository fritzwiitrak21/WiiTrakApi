/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using WiiTrakApi.Repository.Contracts;
using Microsoft.AspNetCore.OData.Query;

namespace WiiTrakApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ILoginRepository Repository;
        private readonly IAuthenticateRepository AuthenticateRepository;

        public LoginController(IMapper mapper,
           ILoginRepository repository, IAuthenticateRepository Authenticaterepository)
        {
            Mapper = mapper;
            Repository = repository;
            AuthenticateRepository = Authenticaterepository;
        }


        [EnableQuery]
        [HttpGet("{Username}/{Password}")]
        public async Task<ActionResult> GetUsersDetailsByLogin(string Username, string Password)
        {
            var login = new LoginDto();
            login.Username = Username;
            login.Password = Password;
            var result = await Repository.GetUsersDetailsByLoginAsync(login);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<UserDto>(result.Users);
            return Ok(dtoList);
        }


        [HttpGet("{Username}")]
        public async Task<ActionResult> GetUsersDetailsByUserName(string Username)
        {
            var forgot = new ForgotPasswordDto();
            forgot.Username = Username;
            var result = await Repository.GetUsersDetailsByUserNameAsync(forgot);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<UserDto>(result.Users);
            return Ok(dtoList);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, ResetPasswordDto reset)
        {
            var result = await Repository.GetUserDetailByIdAsync(id);
            if (!result.IsSuccess || result.User is null)
            {
                return NotFound(result.ErrorMessage);
            }
            result.User.Password = Core.EncryptText(Core.Base64Decrypt(reset.NewPassword));
            result.User.UpdatedAt =
            result.User.PasswordLastUpdatedAt = DateTime.UtcNow;
            result.User.IsFirstLogin = false;

            var updateResult = await Repository.UpdateUserPasswordAsync(result.User);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}
