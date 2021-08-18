using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Category.Handlers.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IResult>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRabbitMqProducer _producer;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, IRabbitMqProducer producer)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _producer = producer;
        }

        public async Task<IResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User
            //     .FindFirst(ClaimTypes.Email)?.Value);
            var user = await _userManager.FindByEmailAsync("ilkerogtc@gmail.com");
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var category = await _categoryRepository.GetAsync(x => x.Name == request.CategoryName && x.Status);
            if (category is null)
            {
                return new ErrorResult(Messages.DataNotFound);
            }

            category.Status = false;
            category.LastModifiedBy = user.UserName;
            category.LastModifiedDate = DateTime.Now;
            _categoryRepository.Update(category);
            var result = await _categoryRepository.SaveChangesAsync();
            if (result <= 0)
            {
                return new ErrorResult(Messages.DeleteFailed);
            }

            await _producer.Publish(new ProducerModel
            {
                Model = category,
                QueueName = "category-deleted-queue"
            });
            return new SuccessResult(Messages.DeletedSuccessfully);
        }
    }
}