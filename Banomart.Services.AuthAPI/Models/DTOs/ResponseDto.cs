using System.Diagnostics.Eventing.Reader;

namespace Banomart.Services.AuthAPI.Models.DTOs
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
