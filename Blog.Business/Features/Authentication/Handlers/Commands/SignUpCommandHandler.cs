using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Transaction;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    /// Sign up admin
    /// </summary>
    [TransactionScopeAspectAsync]
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, IDataResult<SignUpResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationMailService _authenticationMailService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public SignUpCommandHandler(UserManager<User> userManager,
            IAuthenticationMailService authenticationMailService, IMapper mapper, ICloudinaryService cloudinaryService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _authenticationMailService = authenticationMailService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _httpContextAccessor = httpContextAccessor;
        }

        [ValidationAspect(typeof(SignUpValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpCommand request,
            CancellationToken cancellationToken)
        {
            var isUserAlreadyExist = await _userManager.FindByNameAsync(request.SignUpRequest.Username);
            if (isUserAlreadyExist is not null)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.UsernameAlreadyExist);
            }

            var isEmailAlreadyExist = await _userManager.FindByEmailAsync(request.SignUpRequest.Username);
            if (isEmailAlreadyExist is not null)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.EmailAlreadyExist);
            }
            var currentUser = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
                .FindFirst(ClaimTypes.Email)?.Value);

            var user = _mapper.Map<User>(request.SignUpRequest);
            user.CreatedDate = DateTime.Now;
            user.CreatedBy = currentUser.UserName;
            var file = request.SignUpRequest.Image;
            if (file.Length > 0)
            {
                user.Photo = await _cloudinaryService.UploadImage(file);
            }

            user.Status = true;
            var result = await _userManager.CreateAsync(user, request.SignUpRequest.Password);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
                                                           $":{result.Errors.ToList()[0].Description}");
            }
            
            await _userManager.AddToRoleAsync(user, request.SignUpRequest.Role);
            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _authenticationMailService.SendVerificationEmail(user, verificationToken);
            var signupResponse = _mapper.Map<SignUpResponse>(user);
            return new SuccessDataResult<SignUpResponse>(signupResponse, Messages.SignUpSuccessfully);
        }
    }
}