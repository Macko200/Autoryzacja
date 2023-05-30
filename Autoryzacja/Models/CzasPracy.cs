using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Autoryzacja.Models
{
    public class CzasPracy
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public TimeSpan Rozpoczecie { get; set; }
        public TimeSpan Zakonczenie { get; set; }
        public bool PracaZdalna { get; set; }
        public double IloscGodzin { get; set; }
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
