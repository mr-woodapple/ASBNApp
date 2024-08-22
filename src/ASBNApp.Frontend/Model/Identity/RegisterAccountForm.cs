using System.ComponentModel.DataAnnotations;

namespace ASBNApp.Frontend.Model
{
	/// <summary>
	/// User info from identity endpoint to establish claims.
	/// </summary>
	public class RegisterAccountForm
	{
		[Required]
		[EmailAddress]
		public string EMail { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}", ErrorMessage = "Your password needs to have at least 8 characters, one upper case character, one number and one special character.")]
		public string Password { get; set; }

		[Required]
		[Compare(nameof(Password))]
		public string PasswordRepeated { get; set; }
	}
}
