namespace BanoMart.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topicQueueName, string azureServiceBusConnestionString);
    }
}
