using Domain.Publisher.Entities;
using Domain.Publisher.Enums;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Publisher.Services
{
    public class PublisherServices : IPublisherServices
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;
        private readonly string _routingKey;

        public PublisherServices(IConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQConnection:HostName"],
                Port = int.Parse(configuration["RabbitMQConnection:Port"]),
                UserName = configuration["RabbitMQConnection:UserName"],
                Password = configuration["RabbitMQConnection:Password"],
                VirtualHost = configuration["RabbitMQConnection:VirtualHost"]
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _exchangeName = configuration["Exchange"];
            _routingKey = configuration["RoutingKey"];
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);
        }

        public bool SendTo(ToDo data)
        {
            try
            {
                string json = JsonSerializer.Serialize(data);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                _channel.BasicPublish(exchange: _exchangeName, routingKey: $"{_routingKey}.{data.Type}", basicProperties: null, body: bytes);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to publish message: {ex.Message}");
                return false;
            }
        }

        public bool SendTo(int data,EType type)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
                _channel.BasicPublish(exchange: _exchangeName, routingKey: $"{_routingKey}.{type}", basicProperties: null, body: bytes);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to publish message: {ex.Message}");
                return false;
            }
        }
    }
}
