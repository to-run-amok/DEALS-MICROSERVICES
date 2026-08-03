using Microsoft.Extensions.Hosting;

public class RabbitMqHostedService : BackgroundService
{
    private readonly OrderCreatedConsumer _consumer;

    public RabbitMqHostedService(OrderCreatedConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartAsync();
    }
}