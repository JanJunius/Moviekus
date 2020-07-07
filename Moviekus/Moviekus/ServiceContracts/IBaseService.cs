using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface IBaseService<T>
    {
        event EventHandler<T> OnModelInserted;
        event EventHandler<T> OnModelUpdated;
        event EventHandler<T> OnModelDeleted;

        IEnumerable<T> Get();

        Task<IList<T>> GetAsync();

        T Get(string id);

        Task<T> GetAsync(string id);

        Task<T> AddNewAsync(T model);

        Task<T> SaveChangesAsync(T model);

        T SaveChanges(T model);

        Task DeleteAsync(T model);
    }
}
