using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface IRabbitMQService
    {
        void SendCreate(TodoItem item);
    }
}
