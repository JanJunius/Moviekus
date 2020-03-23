using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class BaseService<T> : IService<T> where T : BaseModel, new()
    {
        public event EventHandler<T> OnModelInserted;
        public event EventHandler<T> OnModelUpdated;
        public event EventHandler<T> OnModelDeleted;
        
        public BaseService() 
        {
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Set<T>().ToListAsync();
            }                
        }

        public async Task<T> GetAsync(string id)
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }             
        }

        public virtual async Task<T> SaveChangesAsync(T model)
        {
            if (model.IsNew)
                return await InsertAsync(model);
            return await UpdateAsync(model);
        }

        private async Task<T> InsertAsync(T model)
        {
            using (var context = new MoviekusDbContext())
            {
                await context.Set<T>().AddAsync(model);
                await context.SaveChangesAsync();
                model.IsNew = false;
            }
            OnModelInserted?.Invoke(this, model);
            return model;
        }

        private async Task<T> UpdateAsync(T model)
        {
            using (var context = new MoviekusDbContext())
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            OnModelUpdated?.Invoke(this, model);
            return model;
        }

        public virtual async Task DeleteAsync(T model)
        {
            using (var context = new MoviekusDbContext())
            {
                context.Set<T>().Remove(model);
                //table.Attach(model);
                await context.SaveChangesAsync();
            }
            OnModelDeleted?.Invoke(this, model);
        }

    }
}
