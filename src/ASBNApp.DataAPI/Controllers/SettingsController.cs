using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASBNApp.DataAPI.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class SettingsController : ControllerBase
	{
		private readonly UserManager<User> userManager;

		public SettingsController(UserManager<User> userManager)
		{
			this.userManager = userManager;
		}

		[HttpGet]
		public async Task<ActionResult<Settings>> Get()
		{
			var currentUser = userManager.GetUserAsync(User);

			var userSettings = new Settings()
			{
				GivenName = currentUser.Result.GivenName,
				Surname = currentUser.Result.Surname,
				Username = currentUser.Result.UserName,
				Profession = currentUser.Result.Profession,
				LegalRepresentitive = currentUser.Result.LegalRepresentitive,
				Company = currentUser.Result.Company,
				School = currentUser.Result.School,
				ApprenticeshipStartDate = currentUser.Result.ApprenticeshipStartDate
			};

			return Ok(userSettings);
		}
	}
}
