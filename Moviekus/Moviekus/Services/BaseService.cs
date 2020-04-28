using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public virtual IEnumerable<T> Get()
        {
            using (var context = new MoviekusDbContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Set<T>().ToListAsync();
            }                
        }

        public virtual T Get(string id)
        {
            using (var context = new MoviekusDbContext())
            {
                return context.Set<T>().Find(id);
            }
        }

        public virtual async Task<T> GetAsync(string id)
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }             
        }

        public virtual async Task<T> SaveChangesAsync(T model)
        {
            using (var context = new MoviekusDbContext())
            {
                bool inserted = model.IsNew;
                if (model.IsNew)
                    await InsertAsync(context, model);
                else UpdateAsync(context, model);

                model.IsNew = false;
                await context.SaveChangesAsync();

                if (inserted)
                    OnModelInserted?.Invoke(this, model);
                else OnModelUpdated?.Invoke(this, model);
                return model;
            }
        }

        public virtual T SaveChanges(T model)
        {
            using (var context = new MoviekusDbContext())
            {
                bool inserted = model.IsNew;
                if (model.IsNew)
                    context.Set<T>().Add(model);
                else context.Entry(model).State = EntityState.Modified;

                model.IsNew = false;
                context.SaveChanges();

                if (inserted)
                    OnModelInserted?.Invoke(this, model);
                else OnModelUpdated?.Invoke(this, model);
                return model;
            }
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

        protected virtual async Task InsertAsync(MoviekusDbContext context, T model)
        {
             await context.Set<T>().AddAsync(model);
        }

        protected virtual void UpdateAsync(MoviekusDbContext context, T model)
        {
            context.Entry(model).State = EntityState.Modified;
        }


    }
}
