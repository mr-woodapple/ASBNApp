using Microsoft.AspNetCore.Mvc;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;

namespace ASBNApp.DataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLocationController : ODataController
    {
        private readonly ASBNAppContext _context;

        public WorkLocationController(ASBNAppContext context)
        {
            _context = context;
        }


        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<WorkLocation>> Get()
        {
            return Ok(_context.WorkLocation);
        }
    }
}
