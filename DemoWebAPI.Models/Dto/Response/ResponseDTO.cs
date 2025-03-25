using System.Net;


namespace DemoWebAPI.Models.Dto.Response
{
    public class ResponseDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool RequestSuccess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

    }
}