using ASBNApp.DataAPI.Models;
using ASBNApp.DataAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Deltas;

namespace ASBNApp.DataAPI.Controllers
{
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
            // TODO: Add validation here? (For example check that the strings lenght is ok, etc.)

            _context.Entry.Add(entry);
            _context.SaveChanges();
            return Created(entry);
        }

        [EnableQuery]
        [HttpPatch]
        public ActionResult Patch([FromRoute] int key, [FromBody] Delta<Entry> delta)
        {
            var entry = _context.Entry.SingleOrDefault(d => d.Id == key);

            if (entry == null)
            {
                return NotFound();
            }

            delta.Patch(entry);
            _context.SaveChanges();

            return Updated(entry);
        }
    }
}
