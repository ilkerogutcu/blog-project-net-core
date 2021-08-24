using System.Threading.Tasks;

namespace Blog.Core.Utilities.MessageBrokers.RabbitMq
{
    public interface IRabbitMqProducer
    {
        Task Publish(ProducerModel producerModel);
    }
}