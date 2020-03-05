using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MockSourceService : ISourceService<Source>
    {
        readonly List<Source> sources;

        public MockSourceService()
        {
            sources = new List<Source>()
            {
                new Source { Id = Guid.NewGuid().ToString(), Name = "Lokal" },
                new Source { Id = Guid.NewGuid().ToString(), Name = "Netflix" },
                new Source { Id = Guid.NewGuid().ToString(), Name = "Amazon" }
            };
        }

        public async Task AddSourceAsync(Source source)
        {
            sources.Add(source);

            await Task.FromResult(true);
        }

        public async Task UpdateSourceAsync(Source source)
        {
            var oldSource = sources.Where((Source arg) => arg.Id == source.Id).FirstOrDefault();
            sources.Remove(oldSource);
            sources.Add(source);

            await Task.FromResult(true);
        }

        public async Task DeleteSourceAsync(Source source)
        {
            var oldSource = sources.Where((Source arg) => arg.Id == source.Id).FirstOrDefault();
            sources.Remove(oldSource);

            await Task.FromResult(true);
        }

        public async Task<Source> GetSourceAsync(string id)
        {
            return await Task.FromResult(sources.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Source>> GetSourcesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(sources);
        }
    }
}
