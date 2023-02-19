using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private  IConnection _connection;
    private IModel _chanel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMqHost"],
            Port = int.Parse(_configuration["RabbitMqPort"]!)
        };

        _connection = factory.CreateConnection();
        _chanel = _connection.CreateModel();
        _chanel.ExchangeDeclare(exchange: "trigger" , type: ExchangeType.Fanout);
        _queueName = _chanel.QueueDeclare().QueueName;
        _chanel.QueueBind(queue: _queueName, exchange: "trigger", routingKey:"");

        Console.WriteLine("Listening on the Message bus");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ_ConnectionShutdown");
    }

    public override void Dispose()
    {
        if (_chanel.IsOpen)
        {
            _chanel.Close();
            _connection.Close();
        }
        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_chanel);

        consumer.Received += (moduleHandle, ea) =>
        {
            Console.WriteLine("Event Received");
            var body = ea.Body;
            var notification = Encoding.UTF8.GetString(body.ToArray());
            
            _eventProcessor.ProcessEvent(notification);
        };

        _chanel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        
        return  Task.CompletedTask;
        
    }
}