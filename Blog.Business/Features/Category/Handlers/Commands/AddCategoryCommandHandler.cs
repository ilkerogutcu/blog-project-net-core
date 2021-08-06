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
        public AddCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper,
            UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ICloudinaryService cloudinaryService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryService = cloudinaryService;
        }

        [ValidationAspect(typeof(AddCategoryValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<CategoryDto>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Entities.Concrete.Category>(request);
            var user = await _userManager.FindByEmailAsync("ilkerogtc@gmail.com");
            if (user is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.UserNotFound);
            }

            category.User = user;
            category.CreatedDate = DateTime.Now;
            category.Status = true;
            var file = request.File;
            if (file.Length>0)
            {
                category.Image = await _cloudinaryService.UploadImage(file);
            }
            await _categoryRepository.AddAsync(category);
            var result = await _categoryRepository.SaveChangesAsync();
            if (result <= 0)
            {
                return new ErrorDataResult<CategoryDto>(Messages.AddCategoryFailed);
            }
            
                
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessDataResult<CategoryDto>(categoryDto,Messages.CategorySuccessfullyAdded);
        }
    }
}