using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Moviekus.Models.Validation
{
    public class RatingValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var movie = validationContext.ObjectInstance as Movie;

            if (movie.Rating > 0 && movie.LastSeen == MoviekusDefines.MinDate)
                return new ValidationResult("Es liegt eine Bewertung vor, der Film wurde aber noch nie gesehen.", new string[] { nameof(Movie.Rating) });

            return ValidationResult.Success;
        }
    }
}
