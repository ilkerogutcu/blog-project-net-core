using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Tag.Commands;
using Blog.Business.Features.Tag.ValidationRules;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Tag.Handlers.Commands
{
    /// <summary>
    /// Create a tag
    /// </summary>
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, IDataResult<TagDto>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITagRepository _tagRepository;
        public CreateTagCommandHandler(IMapper mapper, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ITagRepository tagRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _tagRepository = tagRepository;
        }

        [ValidationAspect(typeof(CreateTagValidator))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<TagDto>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Entities.Concrete.Tag>(request);
            var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorDataResult<TagDto>(Messages.UserNotFound);
            }

            tag.User = user;
            tag.CreatedDate=DateTime.Now;
            await _tagRepository.AddAsync(tag);
            var result = await _tagRepository.SaveChangesAsync();
            if (result <= 0) return new ErrorDataResult<TagDto>(Messages.AddFailed);
            var tagDto = _mapper.Map<TagDto>(tag);
            return new SuccessDataResult<TagDto>(tagDto,Messages.DataAddedSuccessfully);
        }
    }
}