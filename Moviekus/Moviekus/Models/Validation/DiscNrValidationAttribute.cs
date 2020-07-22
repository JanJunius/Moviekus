using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Moviekus.Models.Validation
{
    public class DiscNrValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var movie = validationContext.ObjectInstance as Movie;

            // Leere Nr. ist OK, das wird bei der Prüfung der Source gechecked, falls diese auf Disk steht.
            if (!string.IsNullOrEmpty(movie.DiscNumber) && movie.DiscNumber.Length < 3)
                return new ValidationResult("Die Disk-Nr. muss mindestens 3 Zeichen lang sein.", new string[] { nameof(Movie.DiscNumber) });

            if (movie.Source?.Id != MoviekusDefines.SourceDisk && !string.IsNullOrEmpty(movie.DiscNumber))
                return new ValidationResult("Wenn eine Disk-Nr. angegeben ist, muss die Verfügbarkeit auch Disk sein.", new string[] { nameof(Movie.Source) });

            return ValidationResult.Success;
        }
    }
}
