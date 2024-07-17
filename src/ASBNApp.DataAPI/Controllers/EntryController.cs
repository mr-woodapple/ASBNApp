using Microsoft.AspNetCore.Mvc;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;

namespace ASBNApp.DataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ODataController
    {
        private readonly ASBNAppContext _context;

        public EntryController(ASBNAppContext context)
        {
            _context = context;
        }


        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<Entry>> Get()
        {
            return Ok(_context.Entry);
        }

        [EnableQuery]
        [HttpPost]
        public ActionResult Post([FromBody] Entry entry)
        {
            _context.Entry.Add(entry);
            _context.SaveChanges();
            return Created(entry);
        }

        [EnableQuery]
        [HttpPost("PostEntries")]
        public async Task<IActionResult> PostEntries([FromBody] ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (parameters == null)
            {
                return BadRequest();
            }

            var entries = parameters["entries"] as IEnumerable<Entry>;
            await _context.Entry.AddRangeAsync(entries);
            await _context.SaveChangesAsync();

            return Created(entries);
        }
    }
}
