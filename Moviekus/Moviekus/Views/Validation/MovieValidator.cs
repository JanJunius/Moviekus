using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Views.Validation
{
    public class MovieValidator
    {
        public Movie Movie { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        public void Validate()
        {
            ErrorMessage = string.Empty;

            ValidateTitle();
            ValidateDiscNumber();
            ValidateSource();
            ValidateLastSeen();
            ValidateRuntime();
        }

        private bool ValidateTitle()
        {
            if (string.IsNullOrEmpty(Movie.Title))
            {
                ErrorMessage = "Es wurde kein 'Titel' eingebeben.";
                return false;
            }
            return true;
        }

        private bool ValidateSource()
        {
            if (Movie.Source == null)
            {
                ErrorMessage = "'Verfügbar auf' wurde noch nicht ausgewählt.";
                return false;
            }

            if (Movie.Source.Id == MoviekusDefines.SourceDisk && string.IsNullOrEmpty(Movie.DiscNumber))
            {
                ErrorMessage = "'Verfügbar auf' ist 'Disk', aber es wurde keine 'Disk-Nr.' eingegeben.";
                return false;
            }
            return true;
        }

        private bool ValidateDiscNumber()
        {
            if (!string.IsNullOrEmpty(Movie.DiscNumber) && Movie.DiscNumber.Length < 3)
            {
                ErrorMessage = "Die 'Disk-Nr'. sollte mindestens 3 Zeichen lang sein.";
                return false;
            }
            return true;
        }

        private bool ValidateLastSeen()
        {
            if (Movie.Rating > 0 && Movie.LastSeen == MoviekusDefines.MinDate)
            {
                ErrorMessage = "Es liegt eine 'Bewertung' vor, aber 'Zuletzt gesehen' ist nicht gesetzt.";
                return false;
            }
            return true;
        }

        private bool ValidateRuntime()
        {
            if (Movie.Runtime < 1)
            {
                ErrorMessage = "Es wurde keine 'Laufzeit' eingegeben.";
                return false;
            }
            return true;
        }

    }
}
