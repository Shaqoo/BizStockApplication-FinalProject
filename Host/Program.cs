using Application.Extensions;
using Host.Extensions;
using Host.Filters;
using Host.Hubs;
using Infrastructures.Extensions;
using Prometheus;
using System.Text.Json;
using System.Text.Json.Serialization;

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


var app = builder.Build();

app.UseCustomMiddlewares();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseResponseCompression();

app.UseCors("BizStockPolicy");

app.UseHttpsRedirection();

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
