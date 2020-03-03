using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface ISourceService<T>
    {
        Task<bool> AddSourceAsync(T source);
        Task<bool> UpdateSourceAsync(T source);
        Task<bool> DeleteSourceAsync(string id);
        Task<T> GetSourceAsync(string id);
        Task<IEnumerable<T>> GetSourcesAsync(bool forceRefresh = false);
    }
}
