using Banomart.Services.AuthAPI.Models.DTOs;

namespace Banomart.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> RegisterUser(RegistrationDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string userName, string roleName);
    }
}
