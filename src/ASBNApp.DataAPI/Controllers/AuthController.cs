﻿using ASBNApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASBNApp.DataAPI.Controllers;

/// <summary>
/// Extends the functionality the Identity framework provides.
/// </summary>
[Authorize]
[ApiController]
public class AuthController : ControllerBase
{
	/// <summary>
	/// If the data passed alongside the request is null, log out the user by returning unauthorized.
	///
	/// Also forcefully overwriting the AspNetCore Identity cookie, as the delete request isn't coming
	/// through - most likely because I'm using the Azure Managed Functions API on the frontend. 
	/// </summary>
	[HttpPost]
	[Route("/logout")]
	public async Task<IActionResult> Post(SignInManager<User> signInManager, [FromBody] object empty)
	{
		if (empty != null)
		{
			await signInManager.SignOutAsync();
			Response.Cookies.Append(".AspNetCore.Identity.Application", String.Empty, 
				new CookieOptions() { Path = "/", SameSite = SameSiteMode.Lax, Secure = true });
			return Ok();
		}

		return Unauthorized();
	}
}
