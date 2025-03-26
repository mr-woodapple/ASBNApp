namespace ASBNApp.Frontend.Models
{
	/// <summary>
	/// Form using when registering a user.
	/// </summary>
	public class RegisterAccountForm
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordRepeated { get; set; }
	}
}
