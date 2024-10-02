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
		/// If the data passend alongside the request is null, log out the user by returning unauthorized.
		/// </summary>
		[HttpPost]
		[Route("/logout")]
		public async Task<IResult> Post(SignInManager<User> signInManager, [FromBody] object empty)
		{
			if (empty is not null)
			{
				await signInManager.SignOutAsync();
				HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
				return Results.Ok();
			}

			return Results.Unauthorized();
		}
	}
}
