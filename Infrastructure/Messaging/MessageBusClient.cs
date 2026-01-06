using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using testing.Core.Domain.Interfaces;

namespace testing.Infrastructure.Messaging
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IModel? _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = _configuration["RabbitMQHost"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQPort"] ?? "5672"),
                UserName = _configuration["RabbitMQUser"] ?? "admin",
                Password = _configuration["RabbitMQPassword"] ?? "Your_password123"
            };

            try
            {
                // Note: In RabbitMQ.Client 7.0, CreateConnectionAsync is preferred
                // but for this demo we'll use a simplified version.
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to Message Bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishUserCreated(string message)
        {
            if (_connection?.IsOpen == true && _channel != null)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                            routingKey: "",
                            basicProperties: null,
                            body: body);
            
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("Message Bus Disposed");
            if (_channel?.IsOpen == true)
            {
                _channel.Close();
                _connection?.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
