using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RabbitMQ.Client;
using TaskScheduler.Application;
using TaskScheduler.Application.Interfaces;
using TaskScheduler.Domain.Settings;
using TaskScheduler.Infra.MessageBroker;
using TaskScheduler.Infra.MessageBroker.Interfaces;
using TaskScheduler.Infra.Repositories;
using TaskScheduler.Infra.Repositories.Interfaces;

namespace TaskScheduler.Api.ServiceExtensions;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = new AppSettings();
        configuration.Bind(appSettings);

        if (string.IsNullOrEmpty(appSettings.DatabaseSettings?.ConnectionString))
        {
            throw new InvalidOperationException("Missing MongoDB connection string in configuration.");
        }

        services.AddSingleton(appSettings);

        // Fix GuidSerializer cannot serialize a Guid when GuidRepresentation is Unspecified
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


        services.AddSingleton<IMongoClient>(
            new MongoClient(appSettings.DatabaseSettings?.ConnectionString)
        );
        services.AddScoped<IJobRepository, JobRepository>();

#pragma warning disable CS8601 // Possible null reference assignment.
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            HostName = appSettings?.RabbitMqSettings?.HostName
        });
#pragma warning restore CS8601 // Possible null reference assignment.

        services.AddSingleton<IRabbitPublisher, RabbitPublisher>();

        services.AddScoped<IJobService, JobService>();

        return services;
    }
}
