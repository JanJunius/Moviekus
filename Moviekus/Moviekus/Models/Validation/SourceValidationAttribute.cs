using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Moviekus.Models.Validation
{
    public class SourceValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var movie = validationContext.ObjectInstance as Movie;
     
            if (movie.Source == null)
                return new ValidationResult("Es wurde nicht angegeben, wo der Film verfügbar ist.", new string[] { nameof(Movie.Source) });

            if (movie.Source.Id == MoviekusDefines.SourceDisk && string.IsNullOrEmpty(movie.DiscNumber))
                return new ValidationResult("Wenn der Film auf Disk verfügbar ist, muss auch eine Disk-Nummer angegeben werden.", new string[] { nameof(Movie.Source) });

            return ValidationResult.Success;
        }
    }
}
