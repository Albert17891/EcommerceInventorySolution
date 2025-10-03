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
    private string _exchangeName = string.Empty;
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
        var userName = _configuration["RabbitMQ:UserName"] ?? "guest";
        var password = _configuration["RabbitMQ:Password"] ?? "guest";
       _exchangeName = _configuration["RabbitMQ:Exchange"]!;

        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            Port = port,
            UserName = userName,
            Password = password,
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);

        _logger.LogInformation("RabbitMQ connection established and exchange declared: {Exchange}", _exchangeName);

        return (connection, channel);
    }

    public async Task Publish<T>(string routingKey, T message)
    {
        var (connection, channel) = await _connectionLazy.Value;

        var messageBody = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));
       
        await channel.BasicPublishAsync(exchange: _exchangeName,
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
