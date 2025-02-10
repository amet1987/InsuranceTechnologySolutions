using Application.Configuration;
using Application.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Extensions;
using System.Reflection;

namespace Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddRabbitMqBus(configuration);

        services.AddPersistenceServices(configuration);
    }

    public static void ApplyMigrations(this IApplicationBuilder builder)
    {
        builder.ApplyAuditDbMigration();
    }

    private static void AddRabbitMqBus(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = new RabbitMQOptions();
        configuration.GetSection("RabbitMQ").Bind(rabbitMqOptions);

        services.AddMassTransit(x =>
        {
            RegisterConusmers(x);
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Url, rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }

    // Not used added only for demo purpose if we want to change message broker
    private static void AddInMemoryBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            RegisterConusmers(x);

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }

    private static void RegisterConusmers(IRegistrationConfigurator config)
    {
        config.AddConsumersFromNamespaceContaining<ClaimAuditConsumer>();
    }
}
