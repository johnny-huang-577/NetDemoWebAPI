using DemoWebAPI.DataAccess.Data;
using DemoWebAPI.DataAccess.Repository.IRepository;
using DemoWebAPI.Extensions;
using DemoWebAPI.Middlewares;
using DemoWebAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TestBackEnd.DataAccess.Repository;

//=========================================================Configure the app's services.=========================================================

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//配置資料庫連接和服務
builder.Services.AddDbContext<DemoWebAPIDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
));



//註冊 UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 註冊Service
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ITeacherService>()  //掃描 ITeacherService 所在的組件
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))    //選擇以 "Service" 結尾的類別
    .AsImplementedInterfaces()  // 註冊實作的介面
    .WithScopedLifetime()); // 註冊為 Scoped 生命週期

// 註冊 CORS 服務
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:44360")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

//註冊 serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // 從 appsettings.json 讀取配置
    .CreateLogger();

builder.Host.UseSerilog();  // 使用 Serilog 作為日誌提供者


builder.Services.AddControllers().AddJsonOptions(options =>
{
    //當DTO序列化，後所有 null 值的屬性都會被自動忽略，不會出現在 JSON 回應中。
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
}); ;


// 註冊ModelValidation ，用來回覆模型驗證結果
builder.Services.ModelValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "這是我的 API 文件",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your-email@example.com",
            Url = new Uri("https://your-website.com")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});




//=========================================================Configure the HTTP request pipeline.=========================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    });
}

// 使用自訂檢測伺服器異常的 Middleware，用Serilog記錄日誌，以免讓ASP.NET Core內部又再記錄一次
app.UseMiddleware<ServerExceptionMiddleware>();

// 使用 CORS 中介軟體
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
