using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// dotnet WeatherApi.dll로 실행시 appsettings.json 을 찾지 못하므로 아래와 같이 설정해준다.
builder.Configuration.SetBasePath(AppContext.BaseDirectory)
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseKestrel();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler(config => {
    config.Run(async context => {
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        if(exception is ArgumentException argumentException)
        {
            context.Response.StatusCode = 400;
        }
        else if(exception is NullReferenceException)
        {
            context.Response.StatusCode = 404;
        }
        else
        {
            context.Response.StatusCode = 500;
        }
        var result = JsonSerializer.Serialize(new { error = exception?.Message });
        await context.Response.WriteAsync(result);
    });
});
app.Run();
