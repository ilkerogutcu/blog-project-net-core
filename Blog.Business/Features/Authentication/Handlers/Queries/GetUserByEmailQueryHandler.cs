using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
	/// <summary>
	///     Get user by email
	/// </summary>
	public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, IDataResult<UserResponse>>
	{
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;

		public GetUserByEmailQueryHandler(IMapper mapper, UserManager<User> userManager)
		{
			_mapper = mapper;
			_userManager = userManager;
		}

		/// <summary>
		///     Get user by email
		/// </summary>
		public async Task<IDataResult<UserResponse>> Handle(GetUserByEmailQuery request,
			CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user is null) return new ErrorDataResult<UserResponse>(Messages.UserNotFound);

			var userResponse = _mapper.Map<UserResponse>(user);
			return new SuccessDataResult<UserResponse>(userResponse);
		}
	}
}