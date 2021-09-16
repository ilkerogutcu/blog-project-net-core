
using System;

namespace Blog.Core.Entities.DTOs.Authentication.Responses
{
	public class SignUpResponse
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string ImageUrl { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }

	}
}