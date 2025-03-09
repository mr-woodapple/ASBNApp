using ASBNApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASBNApp.DataAPI.Controllers;

/// <summary>
/// Handles retrieving and saving user settings.
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
	private readonly UserManager<User> userManager;

	public SettingsController(UserManager<User> userManager)
	{
		this.userManager = userManager;
	}

	/// <summary>
	/// Retrieves user settings from the identity user model.
	/// This is done so only the required information are passed to the frontend.
	/// </summary>
	/// <returns><see cref="Settings"/> model with requested data.</returns>
	[HttpGet]
	public async Task<ActionResult<Settings>> Get()
	{
		var currentUser = await userManager.GetUserAsync(User);

		var userSettings = new Settings()
		{
			GivenName = currentUser.GivenName,
			Surname = currentUser.Surname,
			Profession = currentUser.Profession,
			LegalRepresentitive = currentUser.LegalRepresentitive,
			Company = currentUser.Company,
			School = currentUser.School,
			ApprenticeshipStartDate = currentUser.ApprenticeshipStartDate
		};

		return Ok(userSettings);
	}
	 
	/// <summary>
	/// Saves the given details back to the identity user model.
	/// </summary>
	/// <param name="settings">Settings model passed along from the frontend.</param>
	/// <returns><see cref="ActionResult"/> Ok() for a successful save, otherwise ValidationProblem().</returns>
	[HttpPatch]
	public async Task<ActionResult> Patch([FromBody] Settings settings)
	{
		var user = await userManager.GetUserAsync(User);

		user.GivenName = settings.GivenName;
		user.Surname = settings.Surname;
		user.Profession = settings.Profession;
		user.LegalRepresentitive = settings.LegalRepresentitive;
		user.Company = settings.Company;
		user.School = settings.School;
		user.ApprenticeshipStartDate = settings.ApprenticeshipStartDate;

		try
		{
			await userManager.UpdateAsync(user);
			return Ok();
		}
		catch
		{
			return ValidationProblem();
		}
	}
}
