namespace TaskScheduler.Infra.MessageBroker.Interfaces;

public interface IRabbitPublisher
{
    Task PublishAsync(string message);
}
