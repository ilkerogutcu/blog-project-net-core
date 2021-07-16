using AutoMapper;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Entities.Concrete;

namespace Blog.Business.Helpers
{
	public class AutoMapperHelper : Profile
	{
		public AutoMapperHelper()
		{
			CreateMap<User, UserResponse>().ReverseMap();
		}
	}
}