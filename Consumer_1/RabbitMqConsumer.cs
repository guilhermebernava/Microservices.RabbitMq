using Domain.Publisher.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQConsumer : IHostedService
{
    private IConnection _connection;
    private IModel _channel;
    private string _exchangeName;
    private string _queueName;

    public RabbitMQConsumer(IConfiguration configuration)
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
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);

        _queueName = _channel.QueueDeclare(exclusive: false, autoDelete: false).QueueName;
        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: $"{_exchangeName}.{EType.Add}");
        _channel.BasicQos(0, 1, false);
    }
    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message Received QUEUE # :({DateTime.Now}) - {message}");

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Close();
        _connection.Close();
        return Task.CompletedTask;
    }
}
