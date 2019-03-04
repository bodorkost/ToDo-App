using System;
using System.Text;
using Core.Entities;
using Core.Settings;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Infrastructure.RabbitMQ
{
    public class RabbitMQClient : IDisposable
    {
        private readonly IOptions<MQCreateSettings> _config;
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private bool disposed = false;

        public RabbitMQClient(IOptions<MQCreateSettings> config)
        {
            _config = config;
            CreateConnection();
        }

        private void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = _config.Value.HostName };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(_config.Value.ExchangeName, _config.Value.Type);

            _model.QueueDeclare(_config.Value.QueueName, true, false, false, null);
            _model.QueueBind(_config.Value.QueueName, _config.Value.ExchangeName, _config.Value.RoutingKey);
        }

        public void SendCreate(TodoItem item)
        {
            var json = JsonConvert.SerializeObject(item);
            SendMessage(Encoding.ASCII.GetBytes(json), _config.Value.RoutingKey);
        }

        public void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(_config.Value.ExchangeName, routingKey, null, message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                _connection.Dispose();
            }

            disposed = true;
        }
    }
}
