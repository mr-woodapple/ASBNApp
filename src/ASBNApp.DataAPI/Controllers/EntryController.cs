﻿using ASBNApp.DataAPI.Models;
using ASBNApp.DataAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ASBNApp.DataAPI.Controllers
{
    [Authorize]
    public class EntryController : ODataController
    {
        private readonly ASBNAppContext _context;
        private readonly UserManager<User> userManager;

        public EntryController(ASBNAppContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }


        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entry>>> Get()
        {
            var currentUser = await userManager.GetUserAsync(User);
            return Ok(_context.Entry.Where(e => e.Owner.Id == currentUser.Id));
        }

        [EnableQuery]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Entry entry)
        {
            // TODO: Add validation here? (For example check that the strings lenght is ok, etc.)

            // TODO: Remove anything the user might have added from the frontend, 
            // only accept the user provided by the backend.
            var currentUser = await userManager.GetUserAsync(User);
            entry.Owner = currentUser;

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

        [EnableQuery]
        [HttpDelete]
        public ActionResult Delete([FromRoute] int key)
        {
            var entry = _context.Entry.SingleOrDefault(d => d.Id == key);

            if (entry != null)
            {
                _context.Entry.Remove(entry);
            }

            _context.SaveChanges();
            return NoContent();
        }
    }
}