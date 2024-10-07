
namespace App.Services.Bus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_Name);
    }
}
