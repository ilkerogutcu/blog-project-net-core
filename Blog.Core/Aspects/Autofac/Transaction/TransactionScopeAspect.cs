using Blog.Core.Utilities.Interceptors;
using Castle.DynamicProxy;
using System;
using System.Transactions;

namespace Blog.Core.Aspects.Autofac.Transaction
{
	/// <summary>
	///     Transaction Scope Aspect
	/// </summary>
	public class TransactionScopeAspect : MethodInterception
	{
		/// <summary>
		/// Transaction scope aspect intercept
		/// </summary>
		/// <param name="invocation">invocation.</param>
		public override void Intercept(IInvocation invocation)
		{
			using var transactionScope = new TransactionScope();
			try
			{
				invocation.Proceed();
				transactionScope.Complete();
			}
			catch (Exception e)
			{
				transactionScope.Dispose();
				throw;
			}
		}
	}
}