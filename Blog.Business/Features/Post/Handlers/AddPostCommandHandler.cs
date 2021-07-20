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
using Blog.Core.Aspects.Autofac.Transaction;
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
    [TransactionScopeAspectAsync]
    public class AddPostCommandHandler : IRequestHandler<AddPostCommand, IResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        
        public AddPostCommandHandler(IMapper mapper, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, 
            IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        [ValidationAspect(typeof(AddPostValidator))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Entities.Concrete.Post>(request);

            var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var category = await _categoryRepository.GetAsync(x => x.Name == request.CategoryName);
            if (category is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            post.Categories = new List<Entities.Concrete.Category> {category};
            post.Tags = new List<Tag>();
            post.CreatedDate = DateTime.Now;
            post.Status = true;
            post.User = user;
            foreach (var tagName in request.Tags)
            {
                var tag = await _tagRepository.GetAsync(x => x.Name == tagName);
                post.Tags.Add(tag);
            }
            await _postRepository.AddAsync(post);
            var result = await _postRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new SuccessResult(Messages.DataAddedSuccessfully);
            }

            return new ErrorResult(Messages.AddFailed);
        }
    }
}