using System.IO;
using Core.Settings;
using MQConsumer.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace MQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build()
                .GetSection("MQCreateSettings")
                .Get<MQCreateSettings>();

            using (var client = new RabbitMQConsumer(config))
            {
                client.ProcessMessages();
            }
        }
    }
}
