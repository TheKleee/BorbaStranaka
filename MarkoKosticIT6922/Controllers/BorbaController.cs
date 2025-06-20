using MarkoKosticIT6922.Data;
using MarkoKosticIT6922.Models;
using MarkoKosticIT6922.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MarkoKosticIT6922.Controllers
{
    [Authorize]
    public class BorbaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorbaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stranke = await _context.Stranke
                .Include(s => s.Argumenti)
                .Where(s => s.Argumenti.Count >= 3)
                .ToListAsync();

            if (stranke.Count < 2) return View("Error");

            var rnd = new Random();
            var stranka1 = stranke[rnd.Next(stranke.Count)];

            Stranka stranka2;
            do
            {
                stranka2 = stranke[rnd.Next(stranke.Count)];
            } while (stranka1.StrankaId == stranka2.StrankaId);

            var model = new BorbaViewModel
            {
                Stranka1Id = stranka1.StrankaId,
                Stranka2Id = stranka2.StrankaId,
                ArgumentiStranka1 = stranka1.Argumenti.OrderBy(x => rnd.Next()).Take(3).ToList(),
                ArgumentiStranka2 = stranka2.Argumenti.OrderBy(x => rnd.Next()).Take(3).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Glasaj(int Stranka1Id, int Stranka2Id, List<int> Glas, List<int> Arg1, List<int> Arg2)
        {
            int glasovaZaStranku1 = Glas.Count(x => x == 1);
            int glasovaZaStranku2 = Glas.Count(x => x == 2);

            int pobednikId = glasovaZaStranku1 > glasovaZaStranku2 ? Stranka1Id : Stranka2Id;
            int gubitnikId = glasovaZaStranku1 > glasovaZaStranku2 ? Stranka2Id : Stranka1Id;

            var borba = new Borba
            {
                Stranka1Id = Stranka1Id,
                Stranka2Id = Stranka2Id,
                PobednikId = pobednikId,
            };

            var pobednik = await _context.Stranke.Where(s => s.StrankaId == pobednikId).FirstAsync();
            var gubitnik = await _context.Stranke.Where(s => s.StrankaId == gubitnikId).FirstAsync();

            _context.Borbe.Add(borba);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = $"Pobednik: {pobednik.Naziv}\nGubitnik: {gubitnik.Naziv}";

            return RedirectToAction("Rezultat");
        }

        public IActionResult Rezultat()
        {
            ViewBag.Rezultat = TempData["Poruka"];
            return View();
        }
    }
}
