using MarkoKosticIT6922.Constants;
using MarkoKosticIT6922.Data;
using MarkoKosticIT6922.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarkoKosticIT6922.Controllers
{

    public class StrankaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StrankaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stranka model)
        {
            if (ModelState.IsValid)
            {
                var s = new Stranka()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                };

                _context.Stranke.Add(s);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var s = await _context.Stranke.ToListAsync();

            return View(s);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var s = await _context.Stranke.FindAsync(id);

            if (s == null) return NotFound();

            return View(s);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Stranka model)
        {
            if (id != model.StrankaId) return NotFound();

            if (ModelState.IsValid)
            {
                var s = await _context.Stranke.FindAsync(id);
                if (s == null) return NotFound(); 

                s.Naziv = model.Naziv;
                s.Opis= model.Opis;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _context.Stranke.FindAsync(id);

            _context.Stranke.Remove(s);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var s = await _context.Stranke
                    .Include(s => s.Argumenti)
                    .ThenInclude(a => a.Glasac)
                    .FirstOrDefaultAsync(s => s.StrankaId == id);

            if (s == null) return NotFound();

            return View(s);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View();
        }
    }
}
