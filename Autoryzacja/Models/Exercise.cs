using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Autoryzacja.Models
{
        public class Exercise
        {
            public int Id { get; set; }
          
            [DisplayName ("Waga[kg]")]
            [Range (0,uint.MaxValue)]
            public int Weight { get; set; }

            [DisplayName("Liczba serii")]
            [Range(0,int.MaxValue)]
            public int NumOfSeries { get; set; }

            [DisplayName("Liczba powtórzeń")]
            public int NumOrReps { get; set; }

            [DisplayName("Informacja od Sesji")]
            public int SessionId { get; set; }

            [DisplayName("Sesja")]
            public virtual Session? Session { get; set; }

            [DisplayName("Typ Ćwiczenia")]
            public int ExerciseTypeId { get; set; }
            [DisplayName("Typ Ćwiczenia")]
            public virtual ExerciseType? ExerciseType { get; set; } // ? - oznacza, ze moze byc NULLem
        }
    }