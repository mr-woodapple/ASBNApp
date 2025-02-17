using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ASBNApp.DataAPI.Controllers;

[Authorize]
public class ImportController : ODataController
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
    /// <param name="jsonDTO">Data to be imported.</param>
    /// <returns>An <see cref="ActionResult"/> with the appropriate reponse code.</returns>
    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] JSONDataWrapperImportDTO jsonDTO)
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

        // Create a new transaction, handle writing data to the model
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (jsonDTO.WorkLocationHours != null)
            {
                // WorkLocations will always be added, not caring if they already exist.
                foreach (var l in jsonDTO.WorkLocationHours)
                {
                    var location = new WorkLocation
                    {
                        Hours = l.Hours,
                        Location = l.Location,
                        Owner = user
                    };

                    if (await _context.WorkLocation.AnyAsync(l => l.Location == location.Location && l.Owner == location.Owner))
                    {
                        Console.WriteLine($"Already location available named {location.Location}, location is discarded.");
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
                // Entries won't be saved if an Entry is already present
                foreach (var e in jsonDTO.Entries)
                {
                    var entry = new Entry
                    {
                        Location = e.Location,
                        Note = e.Note,
                        Date = e.Date,
                        Hours = e.Hours,
                        Owner = user
                    };

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

            // Commit transaction if all commands succeed, transaction will auto-rollback
            // when disposed if either commands fails
            await transaction.CommitAsync();
        }
        catch(Exception e)
        {
            Console.WriteLine($"Import failed with the following message: {e.Message}");
            return BadRequest();
        }

        return Ok();
    }
}
