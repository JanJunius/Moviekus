using Moviekus.Models;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface IGenreService : IBaseService<Genre>
    {
        Genre CreateGenre();

        Task<Genre> GetOrCreateGenre(string genreName);
    }
}
