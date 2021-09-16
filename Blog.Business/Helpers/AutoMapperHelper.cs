﻿using System.Linq;
using AutoMapper;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Post.Commands;
using Blog.Business.Features.Tag.Commands;
using Blog.Core.DataAccess.ElasticSearch.Models;
using Blog.Core.Entities.DTOs.Authentication.Requests;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Photo.Url))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => string.Join("", src.Bio.Take(20))))
                .ReverseMap();
            CreateMap<User, SignUpResponse>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Photo.Url))
                .ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();
            CreateMap<User, SignUpRequest>().ReverseMap();
            CreateMap<Tag, CreateTagCommand>().ReverseMap();
            CreateMap<Category, AddCategoryCommand>().ReverseMap();
            CreateMap<UpdateCategoryCommand, Category>().ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url));
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<ElasticSearchGetModel<Category>, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Item.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Item.User.UserName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Item.Image.Url))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Item.CreatedDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Item.Status))
                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.Item.LastModifiedBy))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.Item.LastModifiedDate))
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
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();
            CreateMap<Tag, TagWithPostsDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}