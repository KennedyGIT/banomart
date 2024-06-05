using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace bano_mart_mvc.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITokenProvider tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) 
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenProvider = tokenProvider;
        }


        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool BearerTokenIsEnabled = true)
        {
            try 
            {
                HttpClient client = httpClientFactory.CreateClient("BanoMartAPI");

                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");

                if (BearerTokenIsEnabled) 
                {
                    var token = tokenProvider.GetToken();

                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (requestDto.HttpMethod)
                {
                    case Utility.Enums.HttpMethod.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Utility.Enums.HttpMethod.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Utility.Enums.HttpMethod.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccessful = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccessful = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccessful = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccessful = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex) 
            {
                return new() { IsSuccessful = false, Message= ex.Message };
            }
            
        }
    }
}
