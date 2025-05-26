namespace TaskScheduler.Domain.Settings;

public class AppSettings
{
    public DatabaseSettings? DatabaseSettings { get; set; }

    public RabbitMqSettings? RabbitMqSettings { get; set;  }
}
