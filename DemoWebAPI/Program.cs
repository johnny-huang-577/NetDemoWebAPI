using DemoWebAPI.DataAccess.Data;
using DemoWebAPI.DataAccess.Repository.IRepository;
using DemoWebAPI.Extensions;
using DemoWebAPI.Middlewares;
using DemoWebAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TestBackEnd.DataAccess.Repository;

//=========================================================Configure the app's services.=========================================================

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//�t�m��Ʈw�s���M�A��
builder.Services.AddDbContext<DemoWebAPIDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
));



//���U UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ���UService
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ITeacherService>()  //���y ITeacherService �Ҧb���ե�
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))    //��ܥH "Service" ���������O
    .AsImplementedInterfaces()  // ���U��@������
    .WithScopedLifetime()); // ���U�� Scoped �ͩR�g��

// ���U CORS �A��
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:44360")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

//���U serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // �q appsettings.json Ū���t�m
    .CreateLogger();

builder.Host.UseSerilog();  // �ϥ� Serilog �@����x���Ѫ�


builder.Services.AddControllers();


// ���UModelValidation �A�ΨӦ^�мҫ����ҵ��G
builder.Services.ModelValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//=========================================================Configure the HTTP request pipeline.=========================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �ϥΦۭq�˴����A�����`�� Middleware�A��Serilog�O����x�A�H�K��ASP.NET Core�����S�A�O���@��
app.UseMiddleware<ServerExceptionMiddleware>();

// �ϥ� CORS �����n��
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
