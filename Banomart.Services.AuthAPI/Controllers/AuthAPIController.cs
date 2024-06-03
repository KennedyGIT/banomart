using Banomart.Services.AuthAPI.Models.DTOs;
using Banomart.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Banomart.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        private ResponseDto responseDto;

        public AuthAPIController(IAuthService authService) 
        {
            this.authService = authService;
            this.responseDto = new();
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto) 
        {
            var errorMessage = await this.authService.RegisterUser(registrationDto);

            if(!string.IsNullOrEmpty(errorMessage)) 
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = errorMessage;
                return BadRequest(responseDto);
            }

            return Ok(responseDto);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await authService.Login(loginRequestDto);

            if (loginResponse.User == null) 
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = "Username or password is incorrect";
                return Unauthorized(responseDto);
            }

            responseDto.Result = loginResponse;
            return Ok(responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {
            var isAssignRoleSuccessful = await authService.AssignRole(assignRoleDto.UserName, assignRoleDto.RoleName.ToUpper());

            if (!isAssignRoleSuccessful)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = "Error Encountered";
                return BadRequest(responseDto);
            }

            return Ok(responseDto);
        }
    }
}
