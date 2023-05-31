using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autoryzacja.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        public DbSet<Autoryzacja.Models.Urlopy>? Urlopy { get; set; }

        public DbSet<Autoryzacja.Models.CzasPracy>? CzasPracy { get; set; }
    }
}