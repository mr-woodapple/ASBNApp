using ASBNApp.Models;
using ASBNApp.DataAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData.Deltas;

namespace ASBNApp.DataAPI.Controllers;

[Authorize]
public class WorkLocationController : ODataController
{
    private readonly ASBNAppContext _context;
	private readonly UserManager<User> _userManager;

    public WorkLocationController(ASBNAppContext context, UserManager<User> userManager)
    {
        _context = context;
		_userManager = userManager;
    }


    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<WorkLocation>>> Get()
    {
		var currentUser = await _userManager.GetUserAsync(User);
		return Ok(_context.WorkLocation.Where(e => e.Owner.Id == currentUser.Id));
    }

    [HttpPost]
	[EnableQuery]
	public async Task<ActionResult> Post([FromBody] WorkLocation workLocation)
    {
        var user = await _userManager.GetUserAsync(User);
        workLocation.Owner = user;

        // TODO: Wrap in a try-catch block
        _context.WorkLocation.Add(workLocation);
        await _context.SaveChangesAsync();
        return Created(workLocation);
    }

    [HttpPatch]
    [EnableQuery]
    public async Task<ActionResult> Patch([FromRoute] int key, [FromBody] Delta<WorkLocation> delta)
    {
        try
        {
			var location = _context.WorkLocation.SingleOrDefault(d => d.Id == key);
			delta.Patch(location);
			await _context.SaveChangesAsync();
			return Updated(location);
		}
        catch
        {
            return NotFound();
        }
	}

    [HttpDelete]
    [EnableQuery]
    public async Task<ActionResult> Delete([FromRoute] int key)
    {
        try
        {
			var location = _context.WorkLocation.SingleOrDefault(e => e.Id == key);
            _context.WorkLocation.Remove(location);
            await _context.SaveChangesAsync();

            return Ok();
		}
        catch
        {
            return Conflict("Cannot delete WorkLocation while it's referenced on one or more Entries.");
        }
	}
}