using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace EcommerceInventory.Infrastructure.RabbitMQ;
public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly Lazy<Task<(IConnection connection, IChannel channel)>> _connectionLazy;

    public RabbitMQPublisher(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _connectionLazy = new Lazy<Task<(IConnection, IChannel)>>(InitAsync);
    }

    private async Task<(IConnection, IChannel)> InitAsync()
    {
        var hostName = _configuration["RabbitMQ:HostName"]!;
        var port = int.Parse(_configuration["RabbitMQ:Port"]!);

        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            Port = port
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        _logger.LogInformation("RabbitMQ connection established");

        return (connection, channel);
    }

    public async Task Publish<T>(string routingKey, T message)
    {
        var (connection, channel) = await _connectionLazy.Value;

        var messageBody = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

        string exchangeName = _configuration["RabbitMQ:ExchangeName"]!;

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
        await channel.BasicPublishAsync(exchange: exchangeName,
                                        routingKey: routingKey,
                                        body: messageBody);

        _logger.LogInformation("Published message to RabbitMQ: {RoutingKey}", routingKey);
    }

    public void Dispose()
    {
        if (_connectionLazy.IsValueCreated)
        {
            var (connection, channel) = _connectionLazy.Value.Result;
            channel?.Dispose();
            connection?.Dispose();
            _logger.LogInformation("RabbitMQ connection closed");
        }
    }
}
