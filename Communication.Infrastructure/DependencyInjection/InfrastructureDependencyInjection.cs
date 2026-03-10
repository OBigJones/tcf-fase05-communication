using Communication.Application.Abstractions.Clients;
using Communication.Infrastructure.Clients;
using Communication.Infrastructure.Messaging;
using Communication.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

            services.AddSingleton<IEmailSender, SmtpEmailSender>();
            services.AddSingleton<RabbitMqConsumer>();
            services.AddHostedService<RabbitMqConsumerHostedService>();

            return services;
        }
    }
}
