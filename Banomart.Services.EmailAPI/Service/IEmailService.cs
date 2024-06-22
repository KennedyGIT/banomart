using Banomart.Services.EmailAPI.Models;

namespace Banomart.Services.EmailAPI.Service
{
    public interface IEmailService
    {
        Task EmailCartLog(CartDto cartDto);
        Task EmailNewUser(UserDto userDto);
    }
}
