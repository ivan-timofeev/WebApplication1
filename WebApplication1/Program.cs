using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Abstraction.Models;
using WebApplication1.Data;
using WebApplication1.Data.Repositories;
using WebApplication1.Common.Middlewares;
using WebApplication1.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.DependencyInjection;
using WebApplication1.Implementation.Helpers;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        x.JsonSerializerOptions.WriteIndented = true;
    });
    
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfiguredAutoMapper();
//builder.Services.AddSingleton<IRepository<Product>, SimpleProductsCrudRepository>();
builder.Services.AddDbContext<WebApplicationDbContext>(
    x => x.UseNpgsql(@"Server=localhost;Database=my_db;Username=postgres;Password=123456;"));
builder.Services.AddSearchEngine();
builder.Services.AddTransient<IRepository<Product>, ProductsRepository>();
builder.Services.AddTransient<IRepository<Manufacturer>, ManufacturersRepository>();
builder.Services.AddTransient<IRepository<SalePoint>, SalePointsRepository>();
builder.Services.AddLogging(x => x.AddConsole());

var globalLogger = new LoggerFactory().CreateLogger("global");
builder.Services.AddSingleton<ILogger>((sp) => globalLogger);

var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();