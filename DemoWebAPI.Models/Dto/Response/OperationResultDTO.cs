using System.Net;


namespace DemoWebAPI.Models.Dto.Response
{
    public class OperationResultDTO
    {
        public bool Success { get; set; } = false;
        public Dictionary<string, string>? Errors { get; set; }
        public object? Data { get; set; }
    }

    //{
    //    "StatusCode": 200,
    //    "IsSuccess": true,
    //    "Data": {
    //      // 各資料的DTO
    //    }
    //}

}