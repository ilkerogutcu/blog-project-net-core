using AutoMapper;
using Blog.Business.Features.Category.Commands;
using Blog.Core.Entities.DTOs.Authentication.Requests;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Entities.Concrete;

namespace Blog.Business.Helpers
{
	public class AutoMapperHelper : Profile
	{
		public AutoMapperHelper()
		{
			CreateMap<User, UserResponse>().ReverseMap();
			CreateMap<User, SignUpRequest>().ReverseMap();
			CreateMap<Category, AddCategoryCommand>().ReverseMap();
		}
	}
}