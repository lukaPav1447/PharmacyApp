using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApotekaApi.Models;

namespace ApotekaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipetypesController : ControllerBase
    {
        private readonly apoteka_dbContext _context;

        public RecipetypesController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: api/Recipetypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipetype>>> GetRecipetypes()
        {
          if (_context.Recipetypes == null)
          {
              return NotFound();
          }
            return await _context.Recipetypes.ToListAsync();
        }

        // GET: api/Recipetypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipetype>> GetRecipetype(int id)
        {
          if (_context.Recipetypes == null)
          {
              return NotFound();
          }
            var recipetype = await _context.Recipetypes.FindAsync(id);

            if (recipetype == null)
            {
                return NotFound();
            }

            return recipetype;
        }

        // PUT: api/Recipetypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipetype(int id, Recipetype recipetype)
        {
            if (id != recipetype.Recipetypeid)
            {
                return BadRequest();
            }

            _context.Entry(recipetype).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipetypeExists(id))
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

        // POST: api/Recipetypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipetype>> PostRecipetype(Recipetype recipetype)
        {
          if (_context.Recipetypes == null)
          {
              return Problem("Entity set 'apoteka_dbContext.Recipetypes'  is null.");
          }
            _context.Recipetypes.Add(recipetype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipetype", new { id = recipetype.Recipetypeid }, recipetype);
        }

        // DELETE: api/Recipetypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipetype(int id)
        {
            if (_context.Recipetypes == null)
            {
                return NotFound();
            }
            var recipetype = await _context.Recipetypes.FindAsync(id);
            if (recipetype == null)
            {
                return NotFound();
            }

            _context.Recipetypes.Remove(recipetype);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipetypeExists(int id)
        {
            return (_context.Recipetypes?.Any(e => e.Recipetypeid == id)).GetValueOrDefault();
        }
    }
}
