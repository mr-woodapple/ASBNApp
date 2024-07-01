using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;

namespace ASBNApp.DataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLocationsController : ControllerBase
    {
        private readonly ASBNAppContext _context;

        public WorkLocationsController(ASBNAppContext context)
        {
            _context = context;
        }

        // GET: api/WorkLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkLocation>>> GetWorkLocation()
        {
            return await _context.WorkLocation.ToListAsync();
        }

        // GET: api/WorkLocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkLocation>> GetWorkLocation(int id)
        {
            var workLocation = await _context.WorkLocation.FindAsync(id);

            if (workLocation == null)
            {
                return NotFound();
            }

            return workLocation;
        }

        // PUT: api/WorkLocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkLocation(int id, WorkLocation workLocation)
        {
            if (id != workLocation.ID)
            {
                return BadRequest();
            }

            _context.Entry(workLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkLocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WorkLocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkLocation>> PostWorkLocation(WorkLocation workLocation)
        {
            _context.WorkLocation.Add(workLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkLocation", new { id = workLocation.ID }, workLocation);
        }

        // DELETE: api/WorkLocations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkLocation(int id)
        {
            var workLocation = await _context.WorkLocation.FindAsync(id);
            if (workLocation == null)
            {
                return NotFound();
            }

            _context.WorkLocation.Remove(workLocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkLocationExists(int id)
        {
            return _context.WorkLocation.Any(e => e.ID == id);
        }
    }
}
