using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Models
{
    public class Settings : BaseModel
    {
        public string MovieDb_ApiKey { get; set; }
        
        public string MovieDb_Language { get; set; }

        public string ImDb_ApiKey { get; set; }

        public string ImDb_Language { get; set; }

        public string OneDrive_ApplicationId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Settings settings &&
                   base.Equals(obj) &&
                   MovieDb_ApiKey == settings.MovieDb_ApiKey &&
                   MovieDb_Language == settings.MovieDb_Language &&
                   ImDb_ApiKey == settings.ImDb_ApiKey &&
                   ImDb_Language == settings.ImDb_Language &&
                   OneDrive_ApplicationId == settings.OneDrive_ApplicationId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), MovieDb_ApiKey, MovieDb_Language, ImDb_ApiKey, ImDb_Language, OneDrive_ApplicationId);
        }
    }
}
