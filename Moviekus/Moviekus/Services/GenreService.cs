using Moviekus.Models;
using Moviekus.ServiceContracts;
using NLog;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class GenreService : BaseService<Genre>, IGenreService
    {
        public async Task<Genre> GetOrCreateGenre(string genreName)
        {
            IEnumerable<Genre> genres = await GetAsync();
            Genre genre = genres.Where(g => g.Name == genreName).FirstOrDefault();

            if (genre != null)
                return genre;

            try
            {
                genre = Genre.CreateNew<Genre>();
                genre.Name = genreName;
                genre = await SaveChangesAsync(genre);

            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
            return genre;
        }

    }
}
