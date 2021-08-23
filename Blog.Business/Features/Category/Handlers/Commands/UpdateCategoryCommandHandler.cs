using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.ValidationRules;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Transaction;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Category.Handlers.Commands
{
    /// <summary>
    /// Update category
    /// </summary>
    [TransactionScopeAspectAsync]
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IDataResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IRabbitMqProducer _producer;
        private readonly ICloudinaryService _cloudinaryService;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, IRabbitMqProducer producer,
            ICloudinaryService cloudinaryService)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _producer = producer;
            _cloudinaryService = cloudinaryService;
        }

        [ValidationAspect(typeof(UpdateCategoryValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<CategoryDto>> Handle(UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            // var user = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User
            //     .FindFirst(ClaimTypes.Email)?.Value);
            var user = await _userManager.FindByEmailAsync("ilkerogtc@gmail.com");
            if (user is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.UserNotFound);
            }

            var category = await _categoryRepository.GetAsync(x => x.Id == request.Id);
            if (category is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.DataNotFound);
            }

            if (request.File is {Length: > 0})
            {
                category.Image = await _cloudinaryService.UploadImage(request.File);
            }
            
            category.Name = request.Name;
            category.Description = request.Description;
            category.LastModifiedBy = user.UserName;
            category.LastModifiedDate = DateTime.Now;

            _categoryRepository.Update(category);
            var result = await _categoryRepository.SaveChangesAsync();
            if (result <= 0)
            {
                return new ErrorDataResult<CategoryDto>(Messages.UpdateFailed);
            }

            await _producer.Publish(new ProducerModel
            {
                Model = category,
                QueueName = "category-updated-queue"
            });

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessDataResult<CategoryDto>(categoryDto, Messages.UpdatedSuccessfully);
        }
    }
}