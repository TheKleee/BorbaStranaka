using MarkoKosticIT6922.Data;
using MarkoKosticIT6922.Models;
using MarkoKosticIT6922.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MarkoKosticIT6922.Controllers
{
    [Authorize]
    public class ArgumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArgumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int strankaId)
        {
            var stranka = await _context.Stranke.FindAsync(strankaId);
            if (stranka != null)
                ViewBag.StrankaNaziv = stranka.Naziv;

            var argument = new Argument
            {
                StrankaId = strankaId
            };

            return View(argument);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Argument model)
        {
            model.GlasacId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var a = new Argument()
                {
                    Tekst = model.Tekst,
                    StrankaId = model.StrankaId,
                    GlasacId = model.GlasacId
                };

                _context.Argumenti.Add(a);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Stranka", new { id = model.StrankaId });
            }

            ViewBag.StrankaNaziv = (await _context.Stranke.FindAsync(model.StrankaId))?.Naziv ?? "Nepoznata stranka";
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var a = await _context.Argumenti.FindAsync(id);

            if (a == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (a.GlasacId != userId && !isAdmin)
                return Forbid();

            var stranka = await _context.Stranke.FindAsync(a.StrankaId);
            if (stranka != null)
                ViewBag.StrankaNaziv = stranka.Naziv;

            return View(a);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Argument model)
        {
            if (id != model.ArgumentId) return NotFound();

            model.GlasacId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var a = await _context.Argumenti.FindAsync(id);
                if (a == null) return NotFound();

                a.Tekst = model.Tekst;
                a.GlasacId = model.GlasacId;
                a.StrankaId = model.StrankaId;
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Stranka", new { id = model.StrankaId });
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var a = await _context.Argumenti.FindAsync(id);

            if(a == null) return NotFound();

            var strankaId = a.StrankaId;

            _context.Argumenti.Remove(a);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Stranka", new { id = strankaId });
        }

        public async Task<IActionResult> Index(int? id, string pretraga, string sortiranje, int page = 1, int pageSize = 5)
        {
            var argumentiUpit = _context.Argumenti
                    .Include(a => a.Stranka)
                    .AsQueryable();

            if (id.HasValue)
                argumentiUpit = argumentiUpit.Where(a => a.StrankaId == id.Value);

            
            if (!string.IsNullOrWhiteSpace(pretraga))
                argumentiUpit = argumentiUpit.Where(a => a.Tekst.Contains(pretraga));

            argumentiUpit = sortiranje switch
            {
                "title_desc" => argumentiUpit.OrderByDescending(a => a.Tekst),
                _ => argumentiUpit.OrderBy(a => a.Tekst),
            };

            var totalItems = await argumentiUpit.CountAsync();

            var argumenti = await argumentiUpit
                .Skip((page-1)*pageSize)
                .Take(pageSize) 
                .ToListAsync();

            var stranke = await _context.Stranke.ToListAsync();

            var selectList = new SelectList(stranke, "StrankaId", "Naziv", id);

            var pagedResult = new PagedResult<Argument>
            {
                Items = argumenti,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            var viewModel = new ArgumentFilterViewModel
            {
                Argumenti = pagedResult,
                StrankaSelektList = selectList,
                SelectedStrankaId = id,
                Pretraga = pretraga,
                Sortiranje = sortiranje
            };

            ViewBag.GlasacId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(viewModel);
        }
    }
}
