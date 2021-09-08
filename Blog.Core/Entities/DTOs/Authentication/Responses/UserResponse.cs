
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
		public string Bio { get; set; }
		public string Username { get; set; }
		public bool Status { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
		public string LastModifiedBy { get; set; }
		public bool EmailConfirmed { get; set; }

	}
}