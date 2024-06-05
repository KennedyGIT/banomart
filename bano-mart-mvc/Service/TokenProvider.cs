using bano_mart_mvc.Service.IService;
using bano_mart_mvc.Utility;

namespace bano_mart_mvc.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Delete(Common.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;

            bool? hasToken = httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(Common.TokenCookie, out token);

            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Append(Common.TokenCookie, token);
        }
    }
}
