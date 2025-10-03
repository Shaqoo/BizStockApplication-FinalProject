using Application.Interfaces.Service;
using Application.Interfaces.Service.Application.Common.Interfaces;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Enums;
using Fido2NetLib;
using Infrastructures.Persistence.Context;
using Infrastructures.Persistence.Repositories;
using Infrastructures.Persistence.UnitOfWork;
using Infrastructures.Service.AuthService;
using Infrastructures.Service.Email;
using Infrastructures.Service.Messaging;
using Infrastructures.Service.Payments;
using Infrastructures.Service.RecoveryCode;
using Infrastructures.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Infrastructures.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BizStockContext>(options => 
              options.UseNpgsql(configuration.GetConnectionString("Default"))
            );

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UserRegisteredEvent>());

            var settings = new ConnectionSettings(new Uri(configuration["Elastic:Url"]!))
                .BasicAuthentication(configuration["Elastic:Username"], configuration["Elastic:Password"])
                .DefaultIndex(configuration["Elastic:Index"]);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);

            services.AddKeyedTransient<IEmailNotificationService, MailjetNotificationService>(EmailNotificationType.Mailjet);
            services.AddKeyedTransient<IEmailNotificationService, BrevoNotificationService>(EmailNotificationType.Brevo);
            
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddDataProtection();

            services.AddScoped<Fido2>(sp =>
            {
                var config = new Fido2Configuration
                {
                    ServerDomain = "localhost",
                    ServerName = "BizStock App",
                    Origins = new HashSet<string> { "http://localhost:5500" } 
                };

                return new Fido2(config);
            });
            

            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.Configure<AiSettings>(configuration.GetSection("AiKeys"));
            services.Configure<AIResourcesSettings>(configuration.GetSection("AIResources"));
            services.Configure<PaystackSettings>(configuration.GetSection("Paystack"));
            services.Configure<FezSettings>(configuration.GetSection("FezSettings"));



            services.Scan(scan => scan
            .FromAssemblyOf<CustomerRepository>()
            .AddClasses(c => c.Where(type => type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            services.Scan(scan => scan
            .FromAssemblyOf<AuthService>()
            .AddClasses(classes => 
            classes.Where(type => type.Name.EndsWith("Service") 
            && !type.GetInterfaces().Any(i => i == typeof(IEmailNotificationService)) ))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            services.AddScoped<IRecoveryCodeGenerator, RecoveryCodeGenerator>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPaymentGatewayService, PaystackPaymentGatewayService>();


            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<WarehouseCreatedConsumer>();
                x.AddConsumer<WarehouseDeactivatedConsumer>();
                x.AddConsumer<ProductCreatedConsumer>();
                x.AddConsumer<StockTransferredConsumer>();
                x.AddConsumer<StockAdjustedConsumer>();
                x.AddConsumer<MfaResetEventConsumer>();
                x.AddConsumer<OrderCreatedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });

                    cfg.ReceiveEndpoint("user-created-queue", e =>
                    {
                        e.ConfigureConsumer<UserCreatedConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));  
                        });
                    });

                    cfg.ReceiveEndpoint("warehouse-created-queue", e =>
                    {
                        e.ConfigureConsumer<WarehouseCreatedConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));
                        });
                    });

                    cfg.ReceiveEndpoint("warehouse-deactivated-queue", e =>
                    {
                        e.ConfigureConsumer<WarehouseDeactivatedConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));
                        });
                    });

                    cfg.ReceiveEndpoint("product-created-queue", e =>
                    {
                        e.ConfigureConsumer<ProductCreatedConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));
                        });
                    });

                    cfg.ReceiveEndpoint("stock-transferred-queue", e =>
                    {
                        e.ConfigureConsumer<StockTransferredConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));
                        });
                    });

                    cfg.ReceiveEndpoint("stock-adjusted-queue", e =>
                    {
                        e.ConfigureConsumer<StockAdjustedConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3,TimeSpan.FromSeconds(10));
                        });
                    });

                    cfg.ReceiveEndpoint("mfa-reset-queue", e =>
                    {
                        e.ConfigureConsumer<MfaResetEventConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));
                        });
                    });

                    cfg.ReceiveEndpoint("order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(10));
                        });

                    });


                });
            });



            return services;
        }
    }
}
