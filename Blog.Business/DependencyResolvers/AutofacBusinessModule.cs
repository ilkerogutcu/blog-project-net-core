using Autofac;
using Autofac.Extras.DynamicProxy;
using Blog.Business.Abstract;
using Blog.Business.Concrete;
using Blog.Core.Utilities.Interceptors;
using Castle.DynamicProxy;
using MediatR;
using System.Reflection;
using FluentValidation;
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