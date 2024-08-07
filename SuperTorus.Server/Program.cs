using SuperTorus.Application.Services;
using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Interfaces;
using SuperTorus.Infrastructure.Data;
using SuperTorus.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SuperTorus.Application.Validation;
using SuperTorus.Application.DTO;
using Serilog;
using SuperTorus.Application.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("any", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"), 
        builder => 
        {
            builder.EnableRetryOnFailure();
        });
});

builder.Services.AddScoped<TorusService>();
builder.Services.AddScoped<TorusDBService>();
builder.Services.AddScoped<IRepository<TorusDB>, TorusRepository>();
builder.Services.AddScoped<IValidator<RequestData>, RequestDataValidator>();


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/Errorlogs.txt", rollingInterval: RollingInterval.Month)
    .CreateLogger();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("any");

app.UseDefaultFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExeptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
