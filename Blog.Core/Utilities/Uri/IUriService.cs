using Blog.Core.Entities.Concrete;

namespace Blog.Core.Utilities.Uri
{
	public interface IUriService
	{
		System.Uri GeneratePageRequestUri(PaginationFilter filter, string route);
	}
}