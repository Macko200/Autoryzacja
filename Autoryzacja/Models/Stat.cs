using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Autoryzacja.Models
{
    public class Stat
    {
        public class Exercises
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<ExerciseResult>? Results { get; set; }

            public class ExerciseResult
            {
            }
        }

        public class ExerciseTypes
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int ExerciseId { get; set; }
            public int Weight { get; set; }
            public int Repetitions { get; set; }
            public DateTime Date { get; set; }
        }

        public class Sessions
        {
            public int Id { get; set; }
            public int UserId { get; set; }
        }
    }
}
