using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class BaseService<T> : IDisposable, IService<T> where T : BaseModel, new()
    {
        public event EventHandler<T> OnModelInserted;
        public event EventHandler<T> OnModelUpdated;
        public event EventHandler<T> OnModelDeleted;
        
        protected readonly DbSet<T> table;

        protected readonly MoviekusDbContext db;

        protected MoviekusDbContext Context => db;

        public BaseService() : this(new MoviekusDbContext())
        {
        }

        public BaseService(MoviekusDbContext context)
        {
            db = context;
            table = db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await table.FindAsync(id);
        }

        public virtual async Task InsertAsync(T model)
        {
            if (model.IsNew)
            {
                await table.AddAsync(model);
                await db.SaveChangesAsync();
                model.IsNew = false;
                OnModelInserted?.Invoke(this, model);
            }
            else await UpdateAsync(model);
        }

        public virtual async Task UpdateAsync(T model)
        {
            if (!model.IsNew)
            {
                T m = table.Find(model.Id);
                if (m != null)
                {
                    m = model;
                    await db.SaveChangesAsync();
                }
                    
                OnModelUpdated?.Invoke(this, model);
            }
            else await InsertAsync(model);
        }

        public virtual async Task DeleteAsync(T model)
        {
            table.Remove(model);
            //table.Attach(model);
            await db.SaveChangesAsync();

            OnModelDeleted?.Invoke(this, model);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
