using Microsoft.Extensions.Hosting;

public class RabbitMqHostedService : BackgroundService
{
    private readonly CropCreatedConsumer _consumer;

    public RabbitMqHostedService(CropCreatedConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartAsync();
    }
}