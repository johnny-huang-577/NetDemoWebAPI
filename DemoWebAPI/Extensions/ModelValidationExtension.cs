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

                    var errors = new Dictionary<string, string>();

                    foreach (var entry in context.ModelState)
                    {
                        var fieldName = entry.Key;

                        // 使用context.ActionDescriptor.Parameters取得當前執行的控制器動作方法的參數
                        // 過濾掉與 DTO 物件名稱匹配的鍵
                        if (context.ActionDescriptor.Parameters.Any(p => p.Name == fieldName))
                        {
                            continue;
                        }

                        // 移除欄位名稱中的前綴，例如 "teacherDetailDTO" 或 "$."
                        fieldName = fieldName.Split('.').Last().TrimStart('$');

                        var errorMessages = entry.Value.Errors;

                        if (errorMessages.Any())
                        {
                            // 檢查是否為型別錯誤
                            var isTypeError = errorMessages.Any(e => e.Exception is FormatException || e.Exception is InvalidCastException)||
                                              errorMessages.Any(e => e.ErrorMessage.Contains("could not be converted"));

                            if (isTypeError)
                            {
                                errors[fieldName] = "資料型別錯誤";
                            }
                            else
                            {
                                errors[fieldName] = errorMessages.First().ErrorMessage;
                            }
                        }
                    }

                    var modelValidationFailedDTO = new ModelValidationFailedDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "必要欄位資料缺失或型別錯誤",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(modelValidationFailedDTO);
                };

            });
        }
    }
}
