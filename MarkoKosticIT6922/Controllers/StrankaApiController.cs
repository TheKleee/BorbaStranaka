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
        public async Task<IActionResult> GetStranke()
        {
            var stranke = await _context.Stranke.ToListAsync();
            return Ok(stranke);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Stranka stranka)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Stranke.Add(stranka);
            await _context.SaveChangesAsync();
            return Ok(stranka);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stranka = await _context.Stranke.FindAsync(id);
            if (stranka == null) return NotFound();

            _context.Stranke.Remove(stranka);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Stranka obrisana." });
        }
    }
}
