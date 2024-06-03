using bano_mart_mvc.Models;

namespace bano_mart_mvc.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationDto registrationDto);  
        Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRoleDto);
    }
}
