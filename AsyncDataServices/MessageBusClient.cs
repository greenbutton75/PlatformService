using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;

            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQHost"],
                Port = Convert.ToInt16(_config["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine($"--> RabbitMQ connected!");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"--> RabbitMQHost connect error: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> RabbitMQ connection Shutdown");
        }

        void IMessageBusClient.PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var msg = JsonSerializer.Serialize(platformPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine($"--> RabbitMQ send msg {msg}");
                SendMessage(msg);
            }
            else
            {
                Console.WriteLine($"--> RabbitMQ connection closed");
            }
        }

        public void Dispose()
        {
            Console.WriteLine($"--> RabbitMQ Dispose");
            if (_channel.IsOpen )
            {
                _channel.Close ();
                _connection.Close ();
            }
        }
        private void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg);
            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);
        }
    }
}