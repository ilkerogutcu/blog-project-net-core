﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog.Core.Utilities.Encryption
{
	public static class SecurityKeyHelper
	{
		public static SecurityKey CreateSecurityKey(string securityKey)
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
		}
	}
}