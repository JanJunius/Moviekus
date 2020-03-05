using System;

namespace Moviekus.Models
{
    public class Movie : BaseModel
    {
        public string Text { get; set; }
        public string Description { get; set; }
    }
}