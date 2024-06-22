
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace BanoMart.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(object message, string topicQueueName, string azureServiceBusConnestionString)
        {
            await using var sbClient = new ServiceBusClient(azureServiceBusConnestionString);

            ServiceBusSender sender = sbClient.CreateSender(topicQueueName);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding
                .UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);

            await sbClient.DisposeAsync();


        }
    }
}
