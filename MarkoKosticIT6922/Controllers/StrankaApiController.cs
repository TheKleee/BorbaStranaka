using MarkoKosticIT6922.Data;
using MarkoKosticIT6922.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarkoKosticIT6922.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StrankaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StrankaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Stranka>>> DohvatiSve()
        {
            var stranke = await _context.Stranke.ToListAsync();
            return Ok(stranke);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Stranka>> DohvatiJednu(int id)
        {
            var jednaStranka = await _context.Stranke.FirstOrDefaultAsync(s => s.StrankaId == id);

            if (jednaStranka == null) return NotFound();

            return Ok(jednaStranka);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Stranka stranka)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if(string.IsNullOrEmpty(stranka.Naziv))
            {
                ModelState.AddModelError("Naziv", "Naziv stranke je obavezan");
                return BadRequest(ModelState);
            }

            var nova = new Stranka
            {
                Naziv = stranka.Naziv,
                Opis = stranka.Opis
            };

            _context.Stranke.Add(nova);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(DohvatiJednu), new { id = nova.StrankaId }, nova);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stranka = await _context.Stranke.FirstOrDefaultAsync(s => s.StrankaId == id);

            if (stranka == null) return NotFound();

            _context.Stranke.Remove(stranka);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
