using System.Text;
using RabbitMQ.Client;
using TaskScheduler.Domain.Settings;
using TaskScheduler.Infra.MessageBroker.Interfaces;

namespace TaskScheduler.Infra.MessageBroker;

public class RabbitPublisher : IRabbitPublisher
{
    private readonly AppSettings _settings;
    private readonly IConnectionFactory _connectionFactory;
    private IChannel? _channel;

    public RabbitPublisher(AppSettings settings, IConnectionFactory connectionFactory)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task PublishAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        }

        message ??= string.Empty;

#pragma warning disable CS8601 // Possible null reference assignment.
        var factory = new ConnectionFactory
        {
            HostName = _settings?.RabbitMqSettings?.HostName,
            UserName = _settings?.RabbitMqSettings?.UserName,
            Password = _settings?.RabbitMqSettings?.Password
        };
#pragma warning restore CS8601 // Possible null reference assignment.

        await using var connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await connection.CreateChannelAsync();

#pragma warning disable CS8604 // Possible null reference argument.
        await _channel.QueueDeclareAsync(
            queue: _settings?.RabbitMqSettings?.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
#pragma warning restore CS8604 // Possible null reference argument.

        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: _settings.RabbitMqSettings.QueueName,
            body: body);
    }
}
