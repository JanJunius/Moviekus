using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface IService<T>
    {
        Task InsertAsync(T source);
        Task UpdateAsync(T source);
        Task DeleteAsync(T source);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAsync();
    }
}
