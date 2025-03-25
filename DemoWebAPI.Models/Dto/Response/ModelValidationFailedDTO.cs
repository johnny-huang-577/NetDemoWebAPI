using System.Net;


namespace DemoWebAPI.Models.Dto.Response
{
    public class ModelValidationFailedDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool RequestSuccess { get; set; } = false;
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }

    //{
    //    "StatusCode": 400,
    //    "IsSuccess": false,
    //    "Message": "提供的JSON資料缺失或格式有誤",
    //    "Error": {
    //        "Error1": "Error message 1",
    //        "Error2": "Error message 2"
    //    }
    //}

}