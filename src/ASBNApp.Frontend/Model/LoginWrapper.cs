namespace ASBNApp.Frontend.Model
{
	/// <summary>
	/// Simple model used to serialize the login data correctly.
	/// </summary>
	public class LoginWrapper
	{
		public string EMail { get; set; }
		public string Password { get; set; }
	}
}
