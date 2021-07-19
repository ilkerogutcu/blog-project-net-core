using Blog.Core.Utilities.IoC;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.DataAccess.DependencyResolvers
{
	public class DataAccessModule : ICoreModule
	{
		public void Load(IServiceCollection serviceCollection)
		{
			serviceCollection.AddDbContext<ApplicationDbContext>();
			serviceCollection.AddSingleton<ICategoryRepository, EfCategoryRepository>();
		}
	}
}