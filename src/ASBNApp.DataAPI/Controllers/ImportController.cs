using ASBNApp.Models;
using ASBNApp.DataAPI.DTOs;
using ASBNApp.DataAPI.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASBNApp.DataAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly ASBNAppContext _context;
    private readonly UserManager<User> _userManager;

    public ImportController(ASBNAppContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Using transactions to import data into the database.
    /// </summary>
    /// <param name="jsonDTO">Entries to be imported.</param>
    /// <returns>An <see cref="ActionResult"/> with the appropriate reponse code.</returns>
    [HttpPost]
	public async Task<ActionResult> Post([FromBody] JSONDataWrapperImportDTO jsonDTO)
    {
		// Create a new transaction, handle writing data to the model
		using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
			var user = await _userManager.GetUserAsync(User);
			if (jsonDTO.Settings != null)
			{
				// Update the user model
				user.GivenName = jsonDTO.Settings.GivenName ?? user.GivenName;
				user.Surname = jsonDTO.Settings.Surname ?? user.Surname;
				user.Profession = jsonDTO.Settings.Profession ?? user.Profession;
				user.LegalRepresentitive = jsonDTO.Settings.LegalRepresentitive ?? user.LegalRepresentitive;
				user.Company = jsonDTO.Settings.Company ?? user.Company;
				user.School = jsonDTO.Settings.School ?? user.School;
				user.ApprenticeshipStartDate = jsonDTO.Settings.ApprenticeshipStartDate ?? user.ApprenticeshipStartDate;

				await _userManager.UpdateAsync(user);
			}

			if (jsonDTO.WorkLocationHours != null)
            {
                // WorkLocations will always be added, not caring if they already exist.
                foreach (var l in jsonDTO.WorkLocationHours)
                {
                    var location = new WorkLocation
                    {
                        SuggestedHours = l.SuggestedHours,
                        LocationName = l.LocationName,
                        Owner = user
                    };

                    if (await _context.WorkLocation.AnyAsync(l => l.LocationName == location.LocationName && l.Owner == location.Owner))
                    {
                        Console.WriteLine($"Already location available named {location.LocationName}, location is discarded.");
                    }
                    else
                    {
                        _context.WorkLocation.Add(location);
                    }
                }
                await _context.SaveChangesAsync();
            }
            
            if (jsonDTO.Entries != null)
            {
                // Load available locations, so we can filter for the correct ones
                var workLocations = _context.WorkLocation.Where(e => e.Owner.Id == user.Id);

				// Entries won't be saved if an Entry is already present
				foreach (var e in jsonDTO.Entries)
                {
                    var entry = new Entry
                    {
						Note = e.Note,
                        Date = e.Date,
                        Hours = e.Hours,
                        Owner = user
                    };

                    // Try assigning a WorkLocation
                    if (e.Location != null)
                    {
						entry.LocationId = workLocations.First(l => l.LocationName == e.Location.LocationName).Id;
					}
                    else if (e.LocationId != null)
					{
						entry.LocationId = workLocations.First(l => l.Id == e.LocationId).Id;
					}
                    else
                    {
						Console.WriteLine($"{nameof(WorkLocation.LocationName)} not found, not saving any location to entry on {e.Date}.");
					}


                    if (await _context.LogEntry.AnyAsync(d => d.Date == entry.Date && d.Owner == entry.Owner))
                    {
                        Console.WriteLine($"Already data available for {entry.Date.Date}, entry is discarded.");
                    }
                    else
                    {
                        _context.LogEntry.Add(entry);
                    }

                }
                await _context.SaveChangesAsync();
			}

			await transaction.CommitAsync();
			return Ok();
		}
		catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Import failed with the following message: {e.Message}");
            return BadRequest();
        }
    }
}
