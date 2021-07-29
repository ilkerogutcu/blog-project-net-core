using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Tag.Commands;
using Blog.Business.Features.Tag.ValidationRules;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Tag.Handlers
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, IResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITagRepository _tagRepository;

        public UpdateTagCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ITagRepository tagRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _tagRepository = tagRepository;
        }

        [ValidationAspect(typeof(UpdateTagValidator))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetAsync(x => x.Id == request.TagId);
            if (tag is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }
            var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            tag.Name = request.Name;
            tag.LastModifiedBy = user.UserName;
            tag.LastModifiedDate=DateTime.Now;
            _tagRepository.Update(tag);
            var result = await _tagRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new SuccessResult(Messages.UpdatedSuccessfully);
            }

            return new ErrorResult(Messages.UpdateFailed);
        }
    }
}