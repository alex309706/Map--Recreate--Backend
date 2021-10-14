using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapWebApi.Models;

namespace MapWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubdivisionsController : ControllerBase
    {
        private readonly SubdivisionsContext _context;

        public SubdivisionsController(SubdivisionsContext context)
        {
            _context = context;
            //инициализация
            if (!_context.Subdivisions.Any())
            {
                Commander Kuznecov = _context.Commanders.FirstOrDefault(x => x.LastName == "Кузнецов");
                TypeOfSubdivision army = _context.TypesOfSubdivision.FirstOrDefault(t => t.Name == "Армия");

                Subdivision armyNumber3 = new Subdivision
                {
                    Commander = Kuznecov,
                    Strength = 212625,
                    Composition = "4ск 11мк    11сд    27сд    56сд    85сд    204мсд  29тд    33тд    7птартб",
                    Name = "3 Армия",
                    TypeOfSubdivision = army
                };
                Commander Korobkov = _context.Commanders.FirstOrDefault(x => x.LastName == "Коробков");
                Subdivision armyNumber4 = new Subdivision
                {
                    Commander = Korobkov,
                    Strength = 212625,
                    Composition = "28ск    14мк    6сд 42сд    49сд    75сд    205мсд  22тд    30тд    10сад",
                    Name = "4 Армия",
                    TypeOfSubdivision = army
                };
                _context.Subdivisions.AddRange(armyNumber3, armyNumber4);
                _context.SaveChanges();
            }

        }

        // GET: api/Subdivisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subdivision>>> GetSubdivisions()
        {
       
            return await _context.Subdivisions
                .Include(s=>s.Commander.Rank)
                .ToListAsync();
        }

        // GET: api/Subdivisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subdivision>> GetSubdivision(int id)
        {
            var subdivision = await _context.Subdivisions.FindAsync(id);

            if (subdivision == null)
            {
                return NotFound();
            }

            return subdivision;
        }

        // PUT: api/Subdivisions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubdivision(int id, Subdivision subdivision)
        {
            if (id != subdivision.Id)
            {
                return BadRequest();
            }

            _context.Entry(subdivision).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubdivisionExists(id))
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

        // POST: api/Subdivisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subdivision>> PostSubdivision(Subdivision subdivision)
        {
            _context.Subdivisions.Add(subdivision);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubdivision", new { id = subdivision.Id }, subdivision);
        }

        // DELETE: api/Subdivisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubdivision(int id)
        {
            var subdivision = await _context.Subdivisions.FindAsync(id);
            if (subdivision == null)
            {
                return NotFound();
            }

            _context.Subdivisions.Remove(subdivision);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubdivisionExists(int id)
        {
            return _context.Subdivisions.Any(e => e.Id == id);
        }
    }
}
