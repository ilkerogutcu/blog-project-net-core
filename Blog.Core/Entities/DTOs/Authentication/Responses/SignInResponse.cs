﻿using System.Collections.Generic;

namespace Blog.Core.Entities.DTOs.Authentication.Responses
{
	public class SignInResponse : IDto
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public List<string> Roles { get; set; }
		public bool TwoStepIsEnabled { get; set; }
		public bool IsVerified { get; set; }
		public string JwtToken { get; set; }
	}
}