using Application.Behaviors;
using Application.Validations.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
        {
            var key = configuration["MediatR:LicenseKey"];
            services.AddMediatR(cfg => {
                cfg.LicenseKey = key; 
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(CrossCuttingBehavior<,>));
            });

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<LoginRequestModelValidator>();

            return services;
        }
    }
}
