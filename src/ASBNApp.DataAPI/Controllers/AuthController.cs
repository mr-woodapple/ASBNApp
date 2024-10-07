using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASBNApp.DataAPI.Controllers
{
	/// <summary>
	/// Extends the functionality the Identity framework provides.
	/// </summary>
	[Authorize]
	[ApiController]
	public class AuthController : ControllerBase
	{
		/// <summary>
		/// If the data passed alongside the request is null, log out the user by returning unauthorized.
		/// </summary>
		[HttpPost]
		[Route("/logout")]
		public async Task<IActionResult> Post(SignInManager<User> signInManager, [FromBody] object empty)
		{
			if (empty != null)
			{
				await signInManager.SignOutAsync();
				return SignOut(IdentityConstants.ApplicationScheme);
			}

			return Unauthorized();
		}
	}
}
