using System;

namespace Moviekus.Models
{
    public class Movie : BaseModel
    {
        public string Title { get; set; }
        
        public string SourceId { get; set; }

        public Source Source { get; set; }
    }
}