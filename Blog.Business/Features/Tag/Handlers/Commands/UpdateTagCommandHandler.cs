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
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, IDataResult<TagDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public UpdateTagCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
            ITagRepository tagRepository, IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(UpdateTagValidator))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<TagDto>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetAsync(x => x.Id==request.Id);
            if (tag is null)
            {
                return new ErrorDataResult<TagDto>(Messages.DataNotFound);
            }

            var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorDataResult<TagDto>(Messages.UserNotFound);
            }

            tag.Name = request.Name;
            tag.LastModifiedBy = user.UserName;
            tag.LastModifiedDate = DateTime.Now;
            _tagRepository.Update(tag);
            var result = await _tagRepository.SaveChangesAsync();
            var tagDto = _mapper.Map<TagDto>(tag);
            if (result > 0)
            {
                return new SuccessDataResult<TagDto>(tagDto,Messages.UpdatedSuccessfully);
            }

            return new ErrorDataResult<TagDto>(Messages.UpdateFailed);
        }
    }
}