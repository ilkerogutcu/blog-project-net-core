using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.ValidationRules;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Category.Handlers.Commands
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, IResult>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AddCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, UserManager<User> userManager)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [ValidationAspect(typeof(CategoryValidator))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Entities.Concrete.Category>(request);
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            category.User = user;
            category.CreatedDate=DateTime.Now;
            category.Image = new Image
            {
                Url = request.ImageUrl,
                CreatedDate = DateTime.Now
            };
           await _categoryRepository.AddAsync(category);
           var result = await _categoryRepository.SaveChangesAsync();
           if (result>=0)
           {
               
               return new SuccessResult(Messages.CategorySuccessfullyAdded);
           }

           return new ErrorResult(Messages.AddCategoryFailed);
        }
    }
}