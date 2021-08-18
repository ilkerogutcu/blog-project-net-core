using System;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Blog.Business.Abstract;
using Blog.Business.Concrete;
using Blog.Core.Utilities.Interceptors;
using Castle.DynamicProxy;
using MediatR;
using System.Reflection;
using Blog.Core.Settings;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using FluentValidation;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Module = Autofac.Module;

namespace Blog.Business.DependencyResolvers
{
    /// <summary>
    ///     Register dependencies for business layer
    /// </summary>
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterType<AuthenticationMailManager>().As<IAuthenticationMailService>().SingleInstance();
            builder.RegisterType<CloudinaryManager>().As<ICloudinaryService>().SingleInstance();
            builder.RegisterType<RabbitMqProducer>().As<IRabbitMqProducer>().SingleInstance();

            builder.Register(context =>
                {
                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(new Uri(RabbitMqSettings.RabbitMqUri), settings =>
                        {
                            settings.Username(RabbitMqSettings.Username);
                            settings.Password(RabbitMqSettings.Password);
                        });
                    });
                    return busControl;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IValidator<>));
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}