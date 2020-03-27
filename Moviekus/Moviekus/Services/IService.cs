using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public interface IService<T>
    {
        Task<T> SaveChangesAsync(T model);
        
        Task DeleteAsync(T model);
        
        IEnumerable<T> Get();
        
        Task<T> GetAsync(string id);
        
        Task<IEnumerable<T>> GetAsync();
    }
}
