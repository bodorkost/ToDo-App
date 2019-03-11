using System;
using System.Text;
using Core.Entities;
using Core.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQConsumer.RabbitMQ
{
    public class RabbitMQConsumer : IDisposable
    {
        private readonly MQCreateSettings _config;
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private bool disposed = false;

        public RabbitMQConsumer(MQCreateSettings config)
        {
            _config = config;
            CreateConnection();
        }

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = _config.HostName };
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine($"Listening for Topic <{_config.RoutingKey}>");
                    Console.WriteLine("------------------------------------------\n");

                    channel.QueueDeclare(_config.QueueName, true, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var json = Encoding.UTF8.GetString(body);
                        var message = (TodoItem)JsonConvert.DeserializeObject(json, typeof(TodoItem));
                        var routingKey = ea.RoutingKey;

                        try
                        {
                            // Dummy condition for nack
                            if (string.IsNullOrEmpty(message.Description))
                            {
                                throw new ArgumentNullException();
                            }

                            Console.WriteLine($"-- Create TodoItem - Routing Key <{routingKey}> : {message.Id} -- {message.Name} -- Ack");
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        catch(ArgumentNullException)
                        {
                            Console.WriteLine($"-- Create TodoItem - Routing Key <{routingKey}> : {message.Id} -- {message.Name} -- Nack (not requeued)");
                            channel.BasicNack(ea.DeliveryTag, false, false);
                        }
                        catch(Exception)
                        {
                            Console.WriteLine($"-- Create TodoItem - Routing Key <{routingKey}> : {message.Id} -- {message.Name} -- Nack (requeued)");
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    };
                    channel.BasicConsume(queue: _config.QueueName, autoAck: false, consumer: consumer);

                    Console.ReadLine();
                }
            }
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