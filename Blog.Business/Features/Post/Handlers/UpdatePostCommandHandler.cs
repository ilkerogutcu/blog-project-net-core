using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Post.Commands;
using Blog.Business.Features.Post.ValidationRules;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Post.Handlers
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, IResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public UpdatePostCommandHandler(IMapper mapper, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        [ValidationAspect(typeof(UpdatePostValidator))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetAsync(x => x.Id == request.PostId);
            if (post is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var category = await _categoryRepository.GetAsync(x => x.Name == request.CategoryName);
            if (category is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            post.Content = request.Content;
            post.Title = request.Title;
            post.Image = new Image
            {
                Url = request.ImageUrl,
                CreatedDate = DateTime.Now
            };
            post.Categories = new List<Entities.Concrete.Category> {category};
            post.Tags = new List<Tag>();
            post.SeoDetail = request.SeoDetail;
            post.Status = request.Status;
            post.LastModifiedBy = user.UserName;
            post.LastModifiedDate = DateTime.Now;
            foreach (var tagName in request.Tags)
            {
                var tag = await _tagRepository.GetAsync(x => x.Name == tagName);
                post.Tags.Add(tag);
            }

            _postRepository.Update(post);
            var result = await _postRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new SuccessResult(Messages.UpdatedSuccessfully);
            }

            return new ErrorResult(Messages.AddFailed);
        }
    }
}