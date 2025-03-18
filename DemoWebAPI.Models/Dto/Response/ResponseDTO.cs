using System.Net;


namespace DemoWebAPI.Models.Dto.Response
{
    public class ResponseDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool RequestSuccess { get; set; }
        public string Message { get; set; }
        public OperationResultDTO? ResponseBody { get; set; }
    }

    //{
    //    "StatusCode": 200,
    //    "IsSuccess": true,
    //    "Data": {
    //      // 各資料的DTO
    //    }
    //}

}