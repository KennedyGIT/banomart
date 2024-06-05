using bano_mart_mvc.Models;

namespace bano_mart_mvc.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool BearerTokenIsEnabled = true);
    }
}
