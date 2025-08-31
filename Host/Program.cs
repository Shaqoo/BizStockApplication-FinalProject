using Application.Extensions;
using Host.Extensions;
using Host.Filters;
using Host.Hubs;
using Infrastructures.Extensions;
using Prometheus;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .Enrich.WithProperty("App Name", "BizStock") 
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .WriteTo.File("logs/logger.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();

        Log.Information("Application started.");

        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

        builder.WebHost.UseWebRoot("wwwroot");
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddHostServices(builder.Configuration);

        builder.Services.AddControllers(options =>
        {
            options.Filters.AddService<SanitizeInputFilter>();
        })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                options.JsonSerializerOptions.AllowTrailingCommas = true;
            });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("BizStockPolicy", policyBuilder =>
            {
                policyBuilder
                    .WithOrigins("http://localhost:5500", "https://c0b8627a5d2c.ngrok-free.app")  
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();

        app.UseCustomMiddlewares();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.UseCors("BizStockPolicy");

        app.UseHttpMetrics();

        app.UseCookiePolicy();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
        app.UseStaticFiles();

        app.MapMetrics();

        app.MapHub<NotificationHub>("/hubs/notificationhub");
        app.MapHub<ChatMessageHub>("/hubs/chatmessagehub");

        app.Run();
    }
}