using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class OrderCreatedConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public OrderCreatedConsumer(
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
            queue: "orders-created",
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
                JsonSerializer.Deserialize<OrderCreatedEvent>(
                    json);

            if (message != null)
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var cropService =
                    scope.ServiceProvider
                        .GetRequiredService<ICropService>();

                Console.WriteLine(
                    $"OrderCreatedEvent received. CropId = {message.CropId}");

                await cropService.MarkAsSoldAsync(
                    message.CropId);
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: "orders-created",
            autoAck: true,
            consumer: consumer);

        Console.WriteLine(
            "Listening for OrderCreatedEvent...");
    }
}