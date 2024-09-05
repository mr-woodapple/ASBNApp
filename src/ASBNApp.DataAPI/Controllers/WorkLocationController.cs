using Microsoft.AspNetCore.Mvc;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
		public async Task<ActionResult> Post ([FromBody] WorkLocation workLocation)
        {
            var user = await _userManager.GetUserAsync(User);
            workLocation.Owner = user;

            // TODO: Wrap in a try-catch block
            _context.WorkLocation.Add(workLocation);
            _context.SaveChanges();
            return Created(workLocation);
        }
    }
}
