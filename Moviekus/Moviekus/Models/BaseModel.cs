using SQLite;

namespace Moviekus.Models
{
    public class BaseModel
    {
        [PrimaryKey]
        public string Id { get; set; }

    }
}
