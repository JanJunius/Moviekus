using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class GenreService : BaseService<Genre>
    {
        public async Task<Genre> GetOrCreateGenre(string genreName)
        {
            IEnumerable<Genre> genres = await GetAsync();
            Genre genre = genres.Where(g => g.Name == genreName).FirstOrDefault();

            if (genre != null)
                return genre;

            genre = Genre.CreateNew<Genre>();
            genre.Name = genreName;

            return await SaveChangesAsync(genre);
        }
    }
}
