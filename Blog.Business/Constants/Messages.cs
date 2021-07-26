
namespace Blog.Business.Constants
{
	/// <summary>
	///     Messages for responses
	/// </summary>
	public static class Messages
	{
		public const string UsernameAlreadyExist = "Username already exist!";
		public const string EmailAlreadyExist = "Username already exist!";
		public const string SignInSuccessfully = "Sign in successfully.";
		public const string RequiredTwoFactoryCode = "Required two factory code!";
		public const string SignInFailed = "Sign in failed!";
		public const string UserNotFound = "User not found!";
		public const string UsernameCannotBeEmpty = "Username cannot be empty!";
		public const string PasswordsDontMatch = "Passwords don't match!";
		public const string PleaseEnterTheEmail = "Please enter the email addres!";
		public const string PleaseEnterAValidEmail = "Please enter a valid email address";
		public const string PleaseEnterTheFirstName = "Please enter the first name!";
		public const string PleaseEnterTheLastName = "Please enter the last name!";
		public const string SignUpFailed = "Sign up failed!";
		public const string ForgotPasswordFailed = "Forgot password failed!";
		public const string SentForgotPasswordEmailSuccessfully = "Sent forgot password email successfully.";
		public const string SentConfirmationEmailSuccessfully = "Verification token has been sent to email successfully";
		public const string Sent2FaCodeEmailSuccessfully = "2FA code has been sent to email successfully";
		public const string EmailIsNotConfirmed = "Email is not confirmed!";
		public const string AuthorizationsDenied = "Authorizations denied!";
		public const string SignUpSuccessfully = "Sign up successfully. Please confirm your account by visiting this URL: ";
		public const string DataNotFound = "Data not found!";
		public const string UsernameIsNotInTheCorrectFormat = "The username field is not in the correct format.";
		public const string TokenCreatedSuccessfully = "Token created successfully.";
		public const string FailedToCreateToken = "Failed to create token!";
		public const string EmailSuccessfullyConfirmed = "Email successfully confirmed.";
		public const string ErrorVerifyingMail = "There was an error verifying email!";
		public const string PasswordHasBeenResetSuccessfully = "Your password has been reset successfully.";
		public const string PasswordResetFailed = "Error occured while reseting the password!";
		public const string FailedToUpdateUser = "Failed to update user!";
		public const string UpdatedUserSuccessfully = "User updated succesfully.";
		public const string GetDateRangeError = "End date must be greater than or equal to start date!";
		public const string FieldMustBeLessThan500Characters = "Field must be less then 500 characters!";
		public const string PleaseEnterTheCategoryName = "Please enter the category name.";
		public const string PleaseEnterTheImageUrl = "Please enter the image url";
		public const string PleaseEnterTheDescription = "Please enter the description";
		public const string CategorySuccessfullyAdded = "Category successfully added";
		public const string AddCategoryFailed = "Category added failed";
		public const string UpdatedSuccessfully = "Updated successfully";
		public const string UpdateFailed = "Update failed!";
		public const string PleaseEnterTheValue = "Please enter the value";
		public const string DataAddedSuccessfully = "Data successfully added";
		public const string AddFailed = "Error occurred while inserting data!";
		public const string YourAccountIsLockedOut = "Your account is locked out. Please wait for 10 minutes and try again";
	}
}