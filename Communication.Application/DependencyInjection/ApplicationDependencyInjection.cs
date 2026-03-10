using Microsoft.Extensions.DependencyInjection;
using Communication.Application.UseCases.Handlers;
using Communication.Application.Abstractions.Clients;
using Communication.Application.Abstractions.Messaging;

namespace Communication.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<SendCommunicationHandler>();
            return services;
        }
    }
}
