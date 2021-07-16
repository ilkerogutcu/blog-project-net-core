using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core.Utilities.IoC
{
	public interface ICoreModule
	{
		void Load(IServiceCollection serviceCollection);
	}
}