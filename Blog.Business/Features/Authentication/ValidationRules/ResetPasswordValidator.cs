using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using FluentValidation;

namespace Blog.Business.Features.Authentication.ValidationRules
{
	/// <summary>
	///     Validator for reset password
	/// </summary>
	public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
	{
		public ResetPasswordValidator()
		{
			RuleFor(x => x.Password).Equal(x => x.ConfirmPassword)
				.WithMessage(Messages.PasswordsDontMatch);
		}
	}
}