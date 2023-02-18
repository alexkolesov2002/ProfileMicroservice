using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMqHost"],
            Port = int.Parse(_configuration["RabbitMqPort"] ?? throw new InvalidOperationException())
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

            Console.WriteLine("Connection to RabbitMq");
        }
        catch (Exception e)
        {
            Console.WriteLine("RabbitMq тютю" + e);
        }
    }

    private void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMq connection shutdown");
    }


    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        
    }
    
}