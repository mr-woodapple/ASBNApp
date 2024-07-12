using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using System.Globalization;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using static MudBlazor.Icons;

namespace ASBNApp.DataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ODataController
    {
        private readonly ASBNAppContext _context;

        public EntriesController(ASBNAppContext context)
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
    }
}
