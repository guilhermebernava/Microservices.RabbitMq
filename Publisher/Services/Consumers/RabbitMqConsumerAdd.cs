using Domain.Publisher.Entities;
using Domain.Publisher.Enums;
using Domain.Repositories;
using Publisher.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

public class RabbitMqConsumerAdd : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private string _exchangeName;
    private string _routingKey;
    private string _queueName;
    private readonly IExampleRepository _repository;

    public RabbitMqConsumerAdd(IConfiguration configuration, IExampleRepository repository)
    {
        _repository = repository;
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

        _queueName = _channel.QueueDeclare("add_queue", exclusive: false, autoDelete: false).QueueName;
        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: $"{_routingKey}.{EType.Add}");
        _channel.BasicQos(0, 1, false);
    }



    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            Console.WriteLine($"Message Received QUEUE ADD :({DateTime.Now})");
            var body = ea.Body.ToArray();
            var toDo = JsonSerializer.Deserialize<ToDo>(Encoding.UTF8.GetString(body));

            if(toDo == null)
            {
                ConsoleUtils.ShowErrorMessage("error in deserialize message");
                _channel.BasicAck(ea.DeliveryTag, false);
                return;
            }

            _repository.Add(toDo!.Example);
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }
}
