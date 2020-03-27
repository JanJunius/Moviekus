using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviekus.Models
{
    /* EF für SQLite kann keine n-m-Beziehungen generieren, daher muss man die Zwischentabelle selbst anlegen
       Beim normalen EF genügt es, in beiden Models eine Collection des jeweils anderen anzulegen
       */
    public class MovieGenre : BaseModel
    {
        /* Ohne die beiden Attribute wird nur eine Spalte MovieId in der Tabelle angelegt
         * Durch ForeignKey wird zusätzlich noch ein FK-Constraint erzeugt
         * Required setzt die Spalte auf NOT NULL
         * */
        [ForeignKey("MovieId")]
        [Required]
        public Movie Movie { get; set; }

        [ForeignKey("GenreId")]
        [Required]
        public Genre Genre { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
