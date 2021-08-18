using Blog.Core.Settings;
using MassTransit;

namespace Blog.Core.Utilities.MessageBrokers.RabbitMq
{
    public static class BusConfigurator
    {
        private static IBusControl _bus;
        public static IBusControl Bus => _bus ??= CreateUsingRabbitMq();

        private static IBusControl CreateUsingRabbitMq()
        {
            var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(x =>
            {
                x.Host(new System.Uri(RabbitMqSettings.RabbitMqUri), h =>
                {
                    h.Username(RabbitMqSettings.Username);
                    h.Password(RabbitMqSettings.Password);
                });
            });
            return bus;
        }
    }
}