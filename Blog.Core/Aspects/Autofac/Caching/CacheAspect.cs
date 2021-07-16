using Blog.Core.CrossCuttingConcerns.Caching;
using Blog.Core.Utilities.Interceptors;
using Blog.Core.Utilities.IoC;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Blog.Core.Aspects.Autofac.Caching
{
	/// <summary>
	/// Cache Aspect
	/// </summary>
	public class CacheAspect : MethodInterception
	{
		private readonly ICacheManager _cacheManager;
		private readonly int _duration;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheAspect"/> class.
		/// </summary>
		/// <param name="duration">Cache expiry duration.</param>
		public CacheAspect(int duration = 20)
		{
			_duration = duration;
			_cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
		}

		/// <summary>
		/// Caching
		/// </summary>
		/// <param name="invocation">invocation.</param>
		public override void Intercept(IInvocation invocation)
		{
			if (invocation.Method.ReflectedType is null)
			{
				return;
			}

			var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
			var arguments = invocation.Arguments.ToList();
			var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
			if (_cacheManager.IsAdd(key))
			{
				invocation.ReturnValue = _cacheManager.Get(key);
				return;
			}

			invocation.Proceed();
			_cacheManager.Add(key, invocation.ReturnValue, _duration);
		}
	}
}