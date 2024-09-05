using Microsoft.AspNetCore.Mvc;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData.Deltas;

namespace ASBNApp.DataAPI.Controllers
{
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
        public ActionResult<IEnumerable<WorkLocation>> Get()
        {
            return Ok(_context.WorkLocation);
        }

        [HttpPost]
		[EnableQuery]
		public async Task<ActionResult> Post([FromBody] WorkLocation workLocation)
        {
            var user = await _userManager.GetUserAsync(User);
            workLocation.Owner = user;

            // TODO: Wrap in a try-catch block
            _context.WorkLocation.Add(workLocation);
            _context.SaveChanges();
            return Created(workLocation);
        }

        [HttpPatch]
        [EnableQuery]
        public async Task<ActionResult> Patch([FromRoute] int key, [FromBody] Delta<Entry> delta)
        {
            try
            {
				var entry = _context.Entry.SingleOrDefault(d => d.Id == key);
				delta.Patch(entry);
				_context.SaveChanges();
				return Updated(entry);
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
				var entry = _context.WorkLocation.SingleOrDefault(e => e.Id == key);
                _context.WorkLocation.Remove(entry);
                _context.SaveChanges();

                return Ok();
			}
            catch
            {
                return NotFound();
            }
		}
    }
}
