namespace Autoryzacja.Models
{
    public class CzasPracyDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Rozpoczecie { get; set; }
        public TimeSpan Zakonczenie { get; set; }
        public bool PracaZdalna { get; set; }
        public int IloscGodzin {get; set; }
    }
}
