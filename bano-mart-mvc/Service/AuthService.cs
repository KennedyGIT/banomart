using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using bano_mart_mvc.Utility;

namespace bano_mart_mvc.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRoleDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = assignRoleDto,
                Url = Common.AuthAPIBase + "/api/auth/AssignRole"
            }, BearerTokenIsEnabled:false);
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = loginRequestDto,
                Url = Common.AuthAPIBase + "/api/auth/Login"
            }, BearerTokenIsEnabled: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationDto registrationDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = registrationDto,
                Url = Common.AuthAPIBase + "/api/auth/Register"
            }, BearerTokenIsEnabled: false );
        }
    }
}
