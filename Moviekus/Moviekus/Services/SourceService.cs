using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class SourceService : BaseService<Source>, ISourceService<Source>
    {
        public Task<Source> GetSourceAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {
            return await GetAsync();
        }

        public async Task AddSourceAsync(Source source)
        {
            await InsertAsync(source);
        }

        public async Task UpdateSourceAsync(Source source)
        {
            await UpdateAsync(source);
        }

        public async Task DeleteSourceAsync(Source source)
        {
            await DeleteAsync(source);
        }


    }
}
