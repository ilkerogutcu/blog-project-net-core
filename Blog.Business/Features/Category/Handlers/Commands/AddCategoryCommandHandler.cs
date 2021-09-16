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
    /// Add a new category
    /// </summary>
    [TransactionScopeAspectAsync]
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, IDataResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IRabbitMqProducer _producer;

        public AddCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper,
            UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
            ICloudinaryService cloudinaryService, IRabbitMqProducer producer)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryService = cloudinaryService;
            _producer = producer;
        }

        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(AddCategoryValidator))]
        public async Task<IDataResult<CategoryDto>> Handle(AddCategoryCommand request,
            CancellationToken cancellationToken)
        {
            if (await _categoryRepository.GetAsync(x => x.Name == request.Name) is not null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.CategoryAlreadyExist);
            }

            var category = _mapper.Map<Entities.Concrete.Category>(request);
            // var user = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
            //     .FindFirst(ClaimTypes.Email)?.Value);
            
            var user = await _userManager.FindByEmailAsync("ilkerogtc@gmail.com");
            if (user is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.UserNotFound);
            }

            category.User = user;
            category.CreatedDate = DateTime.Now;
            category.Status = true;
            var file = request.File;
            if (file.Length > 0)
            {
                category.Image = await _cloudinaryService.UploadImage(file);
            }

            await _categoryRepository.AddAsync(category);
            var result = await _categoryRepository.SaveChangesAsync();
            if (result <= 0)
            {
                return new ErrorDataResult<CategoryDto>(Messages.AddCategoryFailed);
            }

            await _producer.Publish(new ProducerModel
            {
                Model = category,
                QueueName = "category-added-queue"
            });

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessDataResult<CategoryDto>(categoryDto, Messages.CategorySuccessfullyAdded);
        }
    }
}