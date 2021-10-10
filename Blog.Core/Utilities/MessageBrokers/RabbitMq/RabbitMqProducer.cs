using System.Threading.Tasks;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Settings;
using MassTransit;

namespace Blog.Core.Utilities.MessageBrokers.RabbitMq
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly IBus _bus;

        public RabbitMqProducer(IBus bus)
        {
            _bus = bus;
        }

        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task Publish(ProducerModel producerModel)
        {
            // Queue name içeren URI
            var sendToUri = new System.Uri($"{RabbitMqSettings.RabbitMqUri}{producerModel.QueueName}");
            var endPoint = await _bus.GetSendEndpoint(sendToUri);
            await endPoint.Send(producerModel.Model);
        }
    }
}