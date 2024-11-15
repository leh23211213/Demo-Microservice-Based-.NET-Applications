using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
namespace App.Services.Bus
{
    public class MessageBus : IMessageBus
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public MessageBus(
                            IConfiguration configuration
                        )
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ConnectionStrings:DefaultSQLConnection");
        }
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };
            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
