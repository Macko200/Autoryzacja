using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Autoryzacja.Models
{
    public class UrlopyDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Pole Data początkowa jest wymagane.")]
        [Display(Name = "Data początkowa")]
        [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "Data początkowa nie może być wcześniejsza niż aktualna data.")]
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "Pole Data końcowa jest wymagane.")]
        [Display(Name = "Data końcowa")]
        [DataType(DataType.Date)]
        [DateGreaterThan("Start", ErrorMessage = "Data końcowa musi być późniejsza niż data początkowa.")]
        public DateTime End { get; set; }
        public string Type { get; set; }

        public string Status { get; set; }
        public int IloscDni { get; set; }
    }

    public class CurrentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentDate = DateTime.Now.Date;
            var selectedDate = (DateTime)value;

            if (selectedDate < currentDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public DateGreaterThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
            var otherPropertyValue = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance);
            var selectedDate = (DateTime)value;

            if (selectedDate < otherPropertyValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }


}
