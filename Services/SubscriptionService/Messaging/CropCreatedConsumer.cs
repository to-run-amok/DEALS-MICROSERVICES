using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class CropCreatedConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public CropCreatedConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public async Task StartAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:Host"]!
        };

        var connection =
            await factory.CreateConnectionAsync();

        var channel =
            await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "crop-created",
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer =
            new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var json =
                Encoding.UTF8.GetString(
                    ea.Body.ToArray());

            var message =
                JsonSerializer.Deserialize<CropCreatedEvent>(
                    json);

            if (message != null)
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();

                Console.WriteLine($"CropCreatedEvent received. CropId = {message.CropId}");

                await subscriptionService.NotifySubscribersAsync(message);
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: "crop-created",
            autoAck: true,
            consumer: consumer);

        Console.WriteLine("Listening for CropCreatedEvent...");
    }
}