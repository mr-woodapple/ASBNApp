using ASBNApp.Models;
using ASBNApp.DataAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASBNApp.DataAPI.Controllers;

[Authorize]
public class EntryController : ODataController
{
    private readonly ASBNAppContext _context;
    private readonly UserManager<User> userManager;

    public EntryController(ASBNAppContext context, UserManager<User> userManager)
    {
        _context = context;
        this.userManager = userManager;
    }


    [EnableQuery]
    public async Task<ActionResult<IEnumerable<Entry>>> Get()
    {
        var currentUser = await userManager.GetUserAsync(User);
        return Ok(_context.LogEntry.Where(e => e.Owner.Id == currentUser.Id));
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Entry entry)
    {
        // TODO: Add validation here? (For example check that the strings lenght is ok, etc.)

        var user = await userManager.GetUserAsync(User);
        entry.Owner = user;

        // Check if the user has data for this day present, if so this shouldn't be a POST
        if (await _context.LogEntry.AnyAsync(d => d.Date == entry.Date && d.Owner == entry.Owner))
        {
            return Conflict($"Already data available for {entry.Date.Date}, this should be a PATCH request. Request aborted.");
        }
        else 
        {
				_context.LogEntry.Add(entry);
				await _context.SaveChangesAsync();
				return Created(entry);
			} 
    }

    [EnableQuery]
    public ActionResult Patch([FromRoute] int key, [FromBody] Delta<Entry> delta)
    {
        var entry = _context.LogEntry.SingleOrDefault(d => d.Id == key);

        if (entry == null)
        {
            return NotFound();
        }

        delta.Patch(entry);
        _context.SaveChanges();
        return Updated(entry);
    }

    [EnableQuery]
    public ActionResult Delete([FromRoute] int key)
    {
        var entry = _context.LogEntry.SingleOrDefault(d => d.Id == key);

        if (entry != null)
        {
            _context.LogEntry.Remove(entry);
        }

        _context.SaveChanges();
        return NoContent();
    }
}
