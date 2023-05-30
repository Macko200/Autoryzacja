using Autoryzacja.Data;
using Autoryzacja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autoryzacja.Controllers
{
    [Authorize()]
    public class UrlopyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UrlopyController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Pobierz dane z bazy danych lub innych źródeł
            List<Urlopy> urlopyList = await _context.Urlopy.ToListAsync();

            return View(urlopyList);
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlop = await _context.Urlopy.FirstOrDefaultAsync(m => m.Id == id);
            if (urlop == null)
            {
                return NotFound();
            }

            return View(urlop);
        }

       // GET: Sessions/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Sessions/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,End,Type,Status")] UrlopyDTO urlopyDTO)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var urlopy = new Urlopy
            {
                Id = urlopyDTO.Id,
                Start = urlopyDTO.Start.Date,
                End = urlopyDTO.End.Date,
                Type = urlopyDTO.Type,
                Status = "oczekujące",
                UserId = user.Id
            };
            urlopy.Start = DateTime.SpecifyKind(urlopy.Start, DateTimeKind.Unspecified);
            urlopy.End = DateTime.SpecifyKind(urlopy.End, DateTimeKind.Unspecified);

            // Obliczanie liczby dni trwania urlopu
            TimeSpan duration = urlopy.End - urlopy.Start;
            urlopy.IloscDni = duration.Days + 1; // Dodaj 1, aby uwzględnić również pierwszy dzień urlopu

            if (ModelState.IsValid)
            {
                if (urlopy.Start < DateTime.Today)
                {
                    ModelState.AddModelError(string.Empty, "Data początkowa nie może być wcześniejsza niż data obecna.");
                    return View(urlopy);
                }

                if (urlopy.Start > urlopy.End)
                {
                    ModelState.AddModelError(string.Empty, "Data końcowa nie może być wcześniejsza niż data początkowa.");
                    return View(urlopy);
                }

                _context.Add(urlopy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urlopy);
        }


        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlop = await _context.Urlopy.FindAsync(id);
            if (urlop == null)
            {
                return NotFound();
            }
            return View(urlop);
        }

        // POST: Sessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End,Type,Status")] Urlopy urlop)
        {
            if (id != urlop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(urlop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urlop);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlop = await _context.Urlopy.FirstOrDefaultAsync(m => m.Id == id);
            if (urlop == null)
            {
                return NotFound();
            }

            return View(urlop);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urlop = await _context.Urlopy.FindAsync(id);
            if (urlop != null)
            {
                _context.Urlopy.Remove(urlop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
