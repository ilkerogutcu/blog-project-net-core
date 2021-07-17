using System;

namespace Blog.Core.Entities
{
	public interface IEntity
	{
		public DateTime CreatedDate { get; set; }
	}
}