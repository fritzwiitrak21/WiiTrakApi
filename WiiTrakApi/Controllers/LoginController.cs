using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using Microsoft.AspNetCore.OData.Query;
using System.IdentityModel.Tokens.Jwt;

namespace WiiTrakApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _repository;
        private readonly IAuthenticateRepository _authenticateRepository;

        public LoginController(ILogger<LoginController> logger,
           IMapper mapper,
           ILoginRepository repository, IAuthenticateRepository AuthenticateRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _authenticateRepository = AuthenticateRepository;
        }


        [EnableQuery]
        [HttpGet("{Username}/{Password}")]
        public async Task<ActionResult> GetUsersDetailsByLogin(string Username, string Password)
        {
            var login = new LoginDto();
            login.Username = Username;
            login.Password = Password;

            var result = await _repository.GetUsersDetailsByLoginAsync(login);
            //if (result.Users != null)
            //{
            //    var token = _authenticateRepository.Login(login);

            //    var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
            //}




            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<UserDto>(result.Users);
            //dtoList.token = tokenstring;
            return Ok(dtoList);
        }


        [HttpGet("{Username}")]
        public async Task<ActionResult> GetUsersDetailsByUserName(string Username)
        {
            var forgot = new ForgotPasswordDto();
            forgot.Username = Username;


            var result = await _repository.GetUsersDetailsByUserNameAsync(forgot);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<UserDto>(result.Users);
            return Ok(dtoList);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, ResetPasswordDto reset)
        {


            var result = await _repository.GetUserDetailByIdAsync(id);

            if (!result.IsSuccess || result.User is null) return NotFound(result.ErrorMessage);

            result.User.Password = Core.EncryptText(Core.Base64Decrypt(reset.NewPassword));
            result.User.UpdatedAt =
            result.User.PasswordLastUpdatedAt = DateTime.UtcNow;
            result.User.IsFirstLogin = false;

            var updateResult = await _repository.UpdateUserPasswordAsync(result.User);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }
    }
}
