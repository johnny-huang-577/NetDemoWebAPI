using DemoWebAPI.Models.Dto.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using System.Net;

namespace DemoWebAPI.Middlewares
{
    public class ServerExceptionMiddleware
    {
        private readonly RequestDelegate _next;                             // 下一個 Middleware
        private readonly ILogger<ServerExceptionMiddleware> _logger;        // 日誌

        public ServerExceptionMiddleware(RequestDelegate next, ILogger<ServerExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew(); // 記錄請求開始時間

            try
            {
                // 如果下一個 Middleware 有例外，就會進入 catch 區塊
                await _next(context);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();       // 記錄請求結束時間
                var elapsedTime = stopwatch.ElapsedMilliseconds;    // 獲取執行時間 (ms)

                _logger.LogError("====================訊息開始====================");
                var serverExceptionDTO = new ServerExceptionDTO();

                // 根據不同的伺服器例外異常，設定不同的回應
                if (ex is HttpRequestException)       // 後端服務無法連接，設定狀態碼為 502
                {
                    _logger.LogError(ex, "請求時發生異常(HTTP {HttpStatusCode}) | " +
                                         "URL：{Path} | " +
                                         "耗時：{ElapsedTime}ms | " +
                                         "Bad Gateway 無法連接到上游服務，錯誤訊息:{ExceptionMessage}。"
                                         , (int)HttpStatusCode.BadGateway, context.Request.Path, elapsedTime, ex.Message);

                    serverExceptionDTO.StatusCode = HttpStatusCode.BadGateway;
                    serverExceptionDTO.IsSuccess = false;
                    serverExceptionDTO.Messages = "無法連接到服務，請稍後再試。";
                }
                else if (ex is TimeoutException)      // 逾時例外，設定狀態碼為 504
                {
                    _logger.LogError(ex, "請求時發生異常(HTTP {HttpStatusCode}) | " +
                                         "URL：{Path} | " +
                                         "耗時：{ElapsedTime}ms | " +
                                         "Gateway Timeout 伺服器等待上游服務回應超時，錯誤訊息:{ExceptionMessage}。"
                                         , (int)HttpStatusCode.GatewayTimeout, context.Request.Path, elapsedTime, ex.Message);

                    serverExceptionDTO.StatusCode = HttpStatusCode.GatewayTimeout;
                    serverExceptionDTO.IsSuccess = false;
                    serverExceptionDTO.Messages = "伺服器逾時，請稍後再試。";
                }
                else                                  // 其他例外，設定狀態碼為 500
                {
                    _logger.LogError(ex, "請求時發生異常(HTTP {HttpStatusCode}) | " +
                                         "URL：{Path} | " +
                                         "耗時：{ElapsedTime}ms | " +
                                         "Internal Server Error 伺服器發生錯誤，錯誤訊息:{ExceptionMessage}。"
                                         , (int)HttpStatusCode.InternalServerError, context.Request.Path, elapsedTime, ex.Message);

                    serverExceptionDTO.StatusCode = HttpStatusCode.InternalServerError;
                    serverExceptionDTO.IsSuccess = false;
                    serverExceptionDTO.Messages = "伺服器發生錯誤，請稍後再試。";
                }

                _logger.LogError("====================訊息結束====================\n");
                // 設定標頭回應的狀態碼、內容類型
                context.Response.StatusCode = (int)serverExceptionDTO.StatusCode;
                context.Response.ContentType = "application/json";

                // 回應給前端
                await context.Response.WriteAsJsonAsync(serverExceptionDTO);

            }
        }
    }
}
