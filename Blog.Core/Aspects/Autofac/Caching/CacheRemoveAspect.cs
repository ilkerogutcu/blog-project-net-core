using Blog.Core.CrossCuttingConcerns.Caching;
using Blog.Core.Utilities.Interceptors;
using Blog.Core.Utilities.IoC;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core.Aspects.Autofac.Caching
{
	/// <summary>
	/// Remove by pattern from cache
	/// </summary>
	public class CacheRemoveAspect : MethodInterception
	{
		private readonly ICacheManager _cacheManager;
		private readonly string _pattern;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheRemoveAspect"/> class.
		/// </summary>
		/// <param name="pattern">Pattern.</param>
		public CacheRemoveAspect(string pattern)
		{
			_pattern = pattern;
			_cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
		}

		/// <summary>
		/// On success
		/// </summary>
		/// <param name="invocation">invocation.</param>
		protected override void OnSuccess(IInvocation invocation)
		{
			_cacheManager.RemoveByPattern(_pattern);
		}
	}
}