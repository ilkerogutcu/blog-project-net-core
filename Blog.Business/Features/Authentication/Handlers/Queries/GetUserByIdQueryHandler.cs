using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
	/// <summary>
	///     Get user by id
	/// </summary>
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, IDataResult<UserResponse>>
	{
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;

		public GetUserByIdQueryHandler(IMapper mapper, UserManager<User> userManager)
		{
			_mapper = mapper;
			_userManager = userManager;
		}

		/// <summary>
		///     Get user by id
		/// </summary>
		[ExceptionLogAspect(typeof(FileLogger))]
		public async Task<IDataResult<UserResponse>> Handle(GetUserByIdQuery request,
			CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.Id);
			if (user is null) return new ErrorDataResult<UserResponse>(Messages.UserNotFound);

			var userResponse = _mapper.Map<UserResponse>(user);
			return new SuccessDataResult<UserResponse>(userResponse);
		}
	}
}