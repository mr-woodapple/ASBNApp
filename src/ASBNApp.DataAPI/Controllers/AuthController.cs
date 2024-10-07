using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

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
				Response.Cookies.Append(".AspNetCore.Identity.Application", String.Empty, new CookieOptions() { Path = "/", SameSite = SameSiteMode.Lax, Secure = true });
				return Ok();
			}

			return Unauthorized();
		}
	}
}
