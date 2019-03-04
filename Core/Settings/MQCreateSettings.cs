namespace Core.Settings
{
    public class MQCreateSettings
    {
        public string ExchangeName { get; set; }

        public string QueueName { get; set; }

        public string HostName { get; set; }

        public string Type { get; set; }

        public string RoutingKey { get; set; }
    }
}
