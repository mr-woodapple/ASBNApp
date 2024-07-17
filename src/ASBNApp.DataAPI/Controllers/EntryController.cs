using Microsoft.AspNetCore.Mvc;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;

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
        public ActionResult Patch([FromBody] Entry entry)
        {
            throw new NotImplementedException();

            // return Updated(entry);
        }
    }
}
