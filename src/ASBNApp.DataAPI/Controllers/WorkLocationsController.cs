using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;

namespace ASBNApp.DataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLocationsController : ODataController
    {
        private readonly ASBNAppContext _context;

        public WorkLocationsController(ASBNAppContext context)
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
