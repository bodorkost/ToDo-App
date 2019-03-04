using Core.Entities;
using Core.Settings;
using Infrastructure.Interfaces;
using Infrastructure.RabbitMQ;

namespace Infrastructure.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly RabbitMQClient _rabbitMQClient;

        public RabbitMQService(RabbitMQClient rabbitMQClient)
        {
            _rabbitMQClient = rabbitMQClient;
        }

        public void SendCreate(TodoItem item)
        {
            _rabbitMQClient.SendCreate(item);
        }
    }
}
