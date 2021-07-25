using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.ValidationRules;
using Blog.Core.Aspects.Autofac.Exception;
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

namespace Blog.Business.Features.Category.Handlers.Commands
{
    /// <summary>
    /// Update category
    /// </summary>
    [TransactionScopeAspectAsync]
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IResult>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, UserManager<User> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [ValidationAspect(typeof(UpdateCategoryValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var category = await _categoryRepository.GetAsync(x => x.Id == request.Id);
            if (category is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            category.Image = new Image
            {
                Url = request.ImageUrl,
            };
            category.Status = request.Status;
            category.Name = request.Name;
            category.Description = request.Description;
            category.LastModifiedBy = user.UserName;
            category.LastModifiedDate = DateTime.Now;
            
            _categoryRepository.Update(category);
            var result = await _categoryRepository.SaveChangesAsync();
            return result > 0
                ? (IResult) new SuccessResult(Messages.UpdatedSuccessfully)
                : new ErrorResult(Messages.UpdateFailed);
        }
    }
}