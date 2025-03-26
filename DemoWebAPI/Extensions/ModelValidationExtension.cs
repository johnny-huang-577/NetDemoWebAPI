using DemoWebAPI.Models.Dto.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DemoWebAPI.Extensions
{
    // 使用自訂DTO模型回覆驗證結果
    public static class ModelValidationExtension
    {
        public static void ModelValidation(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //資料型態與驗證錯誤之處理
                options.InvalidModelStateResponseFactory = context =>
                {

                    // "$" 是 JSON 格式錯誤的關鍵字
                    // 這裡檢查是否有 JSON 格式錯誤的情況，例如多餘的逗號
                    // 如果有，就直接回傳 JSON 格式錯誤的訊息
                    if (context.ModelState.ContainsKey("$"))
                    {
                        return new BadRequestObjectResult(new ModelValidationFailedDTO
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Message = "JSON 格式錯誤",
                                Errors = new Dictionary<string, string>
                                {
                                    ["format"] = "請檢查 JSON 格式是否正確"
                                }
                        });
                    }


                    // 檢查 ModelState 中的模型欄位綁定錯誤
                    var errors = new Dictionary<string, string>();
                    if (context.ModelState.ErrorCount > 0)
                    {
                        foreach (var entry in context.ModelState.Where(e => e.Value.Errors.Any()))
                        {

                            // 取得欄位名稱
                            var fieldName = entry.Key.Replace("$.", "");
                            // 取得錯誤訊息
                            var errorMessages = entry.Value.Errors;

                            // Action的參數名稱(DTO)不要回傳
                            if (context.ActionDescriptor.Parameters.Any(p => p.Name == fieldName))
                            {
                                continue;
                            }


                            // 處理型別錯誤
                            if (errorMessages.Any(e => e.Exception is FormatException ||
                                                  e.Exception is InvalidCastException ||
                                                  e.ErrorMessage.Contains("could not be converted")))
                            {

                                errors[fieldName] = "資料型別錯誤";
                            }
                            //Model註解驗證的錯誤
                            else
                            {
                                errors[fieldName] = errorMessages.First().ErrorMessage;
                            }
                        }
                    }
                    return new BadRequestObjectResult(new ModelValidationFailedDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "提供的資料不符合要求",
                        Errors = errors
                    });
                };
            });
        }
    }
}
