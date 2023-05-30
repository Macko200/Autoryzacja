using Autoryzacja.Data;
using Autoryzacja.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autoryzacja.Controllers
{
    public class RaportyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RaportyController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(DateTime fromDate, DateTime toDate)
        {
            return RedirectToAction("Raporty", new { fromDate, toDate });
        }

        [HttpGet]
        public IActionResult Raporty(DateTime fromDate, DateTime toDate)
        {
            // Pobierz wszystkich użytkowników
            var users = _context.Users.ToList();

            // Pobierz dane dotyczące czasu pracy dla określonego zakresu dat
            var czasPracyData = _context.CzasPracy
                .Where(cp => cp.Data >= fromDate && cp.Data <= toDate)
                .ToList();

            // Przygotuj listę raportów
            var raporty = new List<RaportViewModel>();

            foreach (var user in users)
            {
                // Pobierz liczbę przepracowanych godzin dla danego użytkownika
                var przepracowaneGodziny = czasPracyData
                    .Where(cp => cp.UserId == user.Id)
                    .Sum(cp => cp.IloscGodzin);

                // Pobierz liczbę przepracowanych godzin zdalnych dla danego użytkownika
                var przepracowaneGodzinyZdalne = czasPracyData
                    .Where(cp => cp.UserId == user.Id && cp.PracaZdalna)
                    .Sum(cp => cp.IloscGodzin);

                // Dodaj raport dla użytkownika do listy raportów
                var raport = new RaportViewModel
                {
                    UserName = user.UserName,
                    PrzepracowaneGodziny = Convert.ToInt32(przepracowaneGodziny),
                    PrzepracowaneGodzinyZdalne = Convert.ToInt32(przepracowaneGodzinyZdalne)
                };

                raporty.Add(raport);
            }

            // Przekazuj listę raportów do widoku
            return View(raporty);
        }
    }
}
