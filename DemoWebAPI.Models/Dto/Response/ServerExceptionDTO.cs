using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebAPI.Models.Dto.Response
{
    public class ServerExceptionDTO
    {
        public HttpStatusCode StatusCode { get; set; }      //HTTP 狀態碼。
        public bool RequestSuccess { get; set; }            //是否成功
        public string Messages { get; set; }                //錯誤訊息
        public DateTime Timestamp { get; set; }             //錯誤發生的時間戳記

        public ServerExceptionDTO()
        {
            Timestamp = DateTime.Now;
        }

    }
}
