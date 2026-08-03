using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

public class RabbitMqPublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync<T>(string queueName,T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:Host"]!
        };

        using var connection = await factory.CreateConnectionAsync();

        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(exchange: "",routingKey: queueName,body: body);
    }
}