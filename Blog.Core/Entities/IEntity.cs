using System;

namespace Blog.Core.Entities
{
	public interface IEntity
	{
		public DateTime CreatedDate { get; set; }
		public DateTime? LastModifiedDate { get; set; }
		public string LastModifiedBy { get; set; }
	}
}