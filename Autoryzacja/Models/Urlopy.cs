using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Autoryzacja.Models
{
    public class Urlopy
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int IloscDni { get; set; }
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
        [NotMapped]
        public string UserName { get; set; } // dodane pole
    }
   

}
