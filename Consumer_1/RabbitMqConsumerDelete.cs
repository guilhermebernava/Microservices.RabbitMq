using Domain.Publisher.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMqConsumerDelete : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private string _exchangeName;
    private string _routingKey;
    private string _queueName;

    public RabbitMqConsumerDelete(IConfiguration configuration)
    {

        var connectionFactory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQConnection:HostName"],
            Port = int.Parse(configuration["RabbitMQConnection:Port"]!),
            UserName = configuration["RabbitMQConnection:UserName"],
            Password = configuration["RabbitMQConnection:Password"],
            VirtualHost = configuration["RabbitMQConnection:VirtualHost"]
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _exchangeName = configuration["Exchange"]!;
        _routingKey = configuration["RoutingKey"]!;
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);

        _queueName = _channel.QueueDeclare("delete_queue",exclusive: false, autoDelete: false).QueueName;
        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: $"{_routingKey}.{EType.Delete}");
        _channel.BasicQos(0, 1, false);
    }
    


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message Received QUEUE DELETE:({DateTime.Now})");

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }
}
