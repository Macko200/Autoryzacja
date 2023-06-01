using Autoryzacja.Data;
using Autoryzacja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autoryzacja.Controllers
{
    [Authorize]
    public class ObslugaUrlopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ObslugaUrlopController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Urlopy> urlopyList = await _context.Urlopy.ToListAsync();

            foreach (var urlop in urlopyList)
            {
                var user = await _userManager.FindByIdAsync(urlop.UserId);
                if (user != null)
                {
                    urlop.UserName = user.UserName;
                }
            }

            return View(urlopyList);
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

            var urlopDTO = new UrlopyDTO
            {
                Id = urlop.Id,
                Start = urlop.Start,
                End = urlop.End,
                Type = urlop.Type,
                Status = urlop.Status
            };

            return View(urlopDTO);
        }
        
        
        // POST: Sessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End,Type,Status")] UrlopyDTO urlopyDTO)
        {
            if (id != urlopyDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUrlop = await _context.Urlopy.FindAsync(id);
                if (existingUrlop == null)
                {
                    return NotFound();
                }

                existingUrlop.Start = urlopyDTO.Start.Date;
                existingUrlop.End = urlopyDTO.End.Date;
                existingUrlop.Type = urlopyDTO.Type;
                existingUrlop.Status = urlopyDTO.Status;

                if (existingUrlop.Start < DateTime.Today)
                {
                    ModelState.AddModelError(string.Empty, "Data początkowa nie może być wcześniejsza niż data obecna.");
                    return View(urlopyDTO);
                }

                if (existingUrlop.Start > existingUrlop.End)
                {
                    ModelState.AddModelError(string.Empty, "Data końcowa nie może być wcześniejsza niż data początkowa.");
                    return View(urlopyDTO);
                }

                // Obliczanie liczby dni trwania urlopu
                TimeSpan duration = existingUrlop.End - existingUrlop.Start;
                existingUrlop.IloscDni = duration.Days + 1; // Dodaj 1, aby uwzględnić również pierwszy dzień urlopu


                _context.Update(existingUrlop);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Zmiany zostały zapisane poprawnie.";
                return RedirectToAction(nameof(Index));
            }

            return View(urlopyDTO);
        }
                   

    }
}