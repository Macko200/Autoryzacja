using Autoryzacja.Data;
using Autoryzacja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoryzacja.Controllers
{
    [Authorize] // Dodany atrybut Authorize do wymagania uwierzytelnienia dla dostępu do kontrolera
    public class CzasPracyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CzasPracyController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); // Pobranie zalogowanego użytkownika
            if (user == null)
            {
                return NotFound();
            }

            // Pobranie danych czasu pracy tylko dla zalogowanego użytkownika
            var czasPracyList = await _context.CzasPracy
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            return View(czasPracyList);
        }

        // GET: CzasPracy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CzasPracy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Rozpoczecie,Zakonczenie,PracaZdalna")] CzasPracyDTO czasPracyDTO)
        {
            var user = await _userManager.GetUserAsync(User); // Pobranie zalogowanego użytkownika
            if (user == null)
            {
                return NotFound();
            }

            var czasPracy = new CzasPracy
            {
                Id = czasPracyDTO.Id,
                Data = czasPracyDTO.Data.Date,
                Rozpoczecie = czasPracyDTO.Rozpoczecie,
                Zakonczenie = czasPracyDTO.Zakonczenie,
                PracaZdalna = czasPracyDTO.PracaZdalna,
                UserId = user.Id
            };
            czasPracy.Data = DateTime.SpecifyKind(czasPracy.Data, DateTimeKind.Unspecified);

            if (ModelState.IsValid)
            {
                if (czasPracy.Rozpoczecie > czasPracy.Zakonczenie)
                {
                    ModelState.AddModelError(string.Empty, "Godzina zakończenia pracy nie może być wcześniejsza niż godzina rozpoczęcia pracy.");
                    return View(czasPracy);
                }

                // Obliczanie ilości przepracowanych godzin
                TimeSpan duration = czasPracy.Zakonczenie - czasPracy.Rozpoczecie;
                czasPracy.IloscGodzin = duration.TotalHours;

                _context.Add(czasPracy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(czasPracy);
        }



        // GET: CzasPracy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var czasPracy = await _context.CzasPracy.FindAsync(id);
            if (czasPracy == null)
            {
                return NotFound();
            }

            // Sprawdzenie, czy użytkownik ma uprawnienia do edycji czasu pracy
            var user = await _userManager.GetUserAsync(User);
            if (czasPracy.UserId != user.Id)
            {
                return Forbid(); // Odmowa dostępu, jeśli użytkownik nie ma uprawnień
            }

            var czasPracyDTO = new CzasPracyDTO
            {
                Id = czasPracy.Id,
                Data = czasPracy.Data,
                Rozpoczecie = czasPracy.Rozpoczecie,
                Zakonczenie = czasPracy.Zakonczenie,
                PracaZdalna = czasPracy.PracaZdalna
            };

            return View(czasPracyDTO);
        }

        // POST: CzasPracy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Rozpoczecie,Zakonczenie,PracaZdalna")] CzasPracyDTO czasPracyDTO)
        {
            if (id != czasPracyDTO.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User); // Pobranie zalogowanego użytkownika
            if (user == null)
            {
                return NotFound();
            }

            var existingCzasPracy = await _context.CzasPracy.FindAsync(id);
            if (existingCzasPracy == null)
            {
                return NotFound();
            }

            // Sprawdzenie, czy użytkownik ma uprawnienia do edycji czasu pracy
            if (existingCzasPracy.UserId != user.Id)
            {
                return Forbid(); // Odmowa dostępu, jeśli użytkownik nie ma uprawnień
            }

            existingCzasPracy.Data = czasPracyDTO.Data.Date;
            existingCzasPracy.Rozpoczecie = czasPracyDTO.Rozpoczecie;
            existingCzasPracy.Zakonczenie = czasPracyDTO.Zakonczenie;
            existingCzasPracy.PracaZdalna = czasPracyDTO.PracaZdalna;

            if (existingCzasPracy.Rozpoczecie > existingCzasPracy.Zakonczenie)
            {
                ModelState.AddModelError(string.Empty, "Godzina zakończenia pracy nie może być wcześniejsza niż godzina rozpoczęcia pracy.");
                return View(czasPracyDTO);
            }

            // Obliczanie ilości przepracowanych godzin
            TimeSpan duration = existingCzasPracy.Zakonczenie - existingCzasPracy.Rozpoczecie;
            existingCzasPracy.IloscGodzin = duration.TotalHours;

            _context.Update(existingCzasPracy);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Zmiany zostały zapisane poprawnie.";
            return RedirectToAction(nameof(Index));
        }

         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var czasPracy = await _context.CzasPracy.FirstOrDefaultAsync(m => m.Id == id);
            if (czasPracy == null)
            {
                return NotFound();
            }

            return View(czasPracy);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var czasPracy = await _context.CzasPracy.FirstOrDefaultAsync(m => m.Id == id);
            if (czasPracy == null)
            {
                return NotFound();
            }

            return View(czasPracy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var czasPracy = await _context.CzasPracy.FindAsync(id);
            if (czasPracy != null)
            {
                _context.CzasPracy.Remove(czasPracy);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        

    }
}
