
using System;

namespace Blog.Core.Entities.DTOs.Authentication.Responses
{
	public class UserResponse
	{
		public string Id { get; set; }
		public string ImageUrl { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
		public DateTime LastModifiedBy { get; set; }

	}
}