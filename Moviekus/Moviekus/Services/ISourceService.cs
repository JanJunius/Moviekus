using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface ISourceService<T>
    {
        Task AddSourceAsync(T source);
        Task UpdateSourceAsync(T source);
        Task DeleteSourceAsync(T source);
        Task<T> GetSourceAsync(string id);
        Task<IEnumerable<T>> GetSourcesAsync(bool forceRefresh = false);
    }
}
