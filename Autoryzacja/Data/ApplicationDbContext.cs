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

        public DbSet<Autoryzacja.Models.ExerciseType> ExerciseType { get; set; } = default!;

        public DbSet<Autoryzacja.Models.Exercise>? Exercise { get; set; }

        public DbSet<Autoryzacja.Models.Session>? Session { get; set; }

        public DbSet<Autoryzacja.Models.Urlopy>? Urlopy { get; set; }
    }
}