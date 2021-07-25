using AutoMapper;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Post.Commands;
using Blog.Core.Entities.DTOs.Authentication.Requests;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;

namespace Blog.Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, SignUpRequest>().ReverseMap();
            CreateMap<Category, AddCategoryCommand>().ReverseMap();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<Post, AddPostCommand>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ReverseMap();
            CreateMap<Post, UpdatePostCommand>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();
        }
    }
}