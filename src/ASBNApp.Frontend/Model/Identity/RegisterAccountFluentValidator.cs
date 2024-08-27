using FluentValidation;

namespace ASBNApp.Frontend.Model.Identity
{
	/// <summary>
	/// FluentValidation validator to perform in-browser validation when registering a new user.
	/// </summary>
	public class RegisterAccountFluentValidator : AbstractValidator<RegisterAccountForm>
	{
		public RegisterAccountFluentValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress()
				.WithName("E-Mail")
				.WithMessage("E-Mail needs to be in a valid format (e.g. info@mail.com)!");

			RuleFor(x => x.Password)
				.NotEmpty()
				.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}")
				.WithName("Password")
				.WithMessage("Password needs to be min 8 characters, contains at least 1 uppercase letter, 1 lowercase letter, 1 number and one special character.");

			RuleFor(x => x.PasswordRepeated)
				.NotEmpty()
				.Equal(x => x.Password)
				.WithName("Repeated password")
				.WithMessage("Passwords do not match!");
		}
	

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<RegisterAccountForm>.CreateWithOptions((RegisterAccountForm)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
