using Blog.Core.Utilities.Interceptors;
using Blog.Core.Utilities.IoC;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace Blog.Core.Aspects.Autofac.Transaction
{
	/// <summary>
	/// Transaction scope aspect async
	/// </summary>
	public class TransactionScopeAspectAsync : MethodInterception
	{
		private readonly Type _dbContextType;

		/// <summary>
		/// Transaction scope aspect intercept DB Context
		/// </summary>
		/// <param name="invocation">invocation.</param>
		public void InterceptDbContext(IInvocation invocation)
		{
			var db = ServiceTool.ServiceProvider.GetService(_dbContextType) as DbContext;
			using var transactionScope = db?.Database.BeginTransaction();
			try
			{
				invocation.Proceed();
				transactionScope?.Commit();
			}
			finally
			{
				transactionScope?.Rollback();
			}
		}

		/// <summary>
		/// Transaction scope aspect intercept
		/// </summary>
		/// <param name="invocation">invocation.</param>
		public override void Intercept(IInvocation invocation)
		{
			using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
			invocation.Proceed();
			tx.Complete();
		}
	}
}