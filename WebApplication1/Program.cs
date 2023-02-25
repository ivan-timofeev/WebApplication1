using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using WebApplication1.Common.Middlewares;
using WebApplication1.Implementation.BackgroundTasks;
using WebApplication1.Implementation.Helpers.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        x.JsonSerializerOptions.WriteIndented = true;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddHostedService<CheckExpiredOrdersBackgroundTask>();

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

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


var staticFileOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        root: Path.Combine(builder.Environment.ContentRootPath, "files")),
    RequestPath = "/files"
};
app.UseStaticFiles(staticFileOptions);

app.Run();
