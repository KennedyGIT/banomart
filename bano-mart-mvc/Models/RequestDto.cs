using HttpMethod = bano_mart_mvc.Utility.Enums.HttpMethod;

namespace bano_mart_mvc.Models
{
    public class RequestDto
    {
        public HttpMethod HttpMethod { get; set; } = HttpMethod.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
