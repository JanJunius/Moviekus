using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Moviekus.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseModel, new()
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

        public virtual async Task<IList<T>> GetAsync()
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

        public virtual async Task<T> AddNewAsync(T model)
        {
            model.IsNew = true;
            return await SaveChangesAsync(model);
        }

        public virtual async Task<T> SaveChangesAsync(T model)
        {
            if (model.IsDeleted)
                return model;

            using (var context = new MoviekusDbContext())
            {
                try
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
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Error(ex);
                    throw;
                }
                return model;
            }
        }

        public virtual T SaveChanges(T model)
        {
            using (var context = new MoviekusDbContext())
            {
                try
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
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Error(ex);
                    throw;  
                }
                return model;
            }
        }

        public virtual async Task DeleteAsync(T model)
        {
            if (!model.IsNew)
            {
                using (var context = new MoviekusDbContext())
                {
                    try
                    {
                        context.Set<T>().Remove(model);
                        DeleteAsync(context, model);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetCurrentClassLogger().Error(ex);
                        throw;
                    }
                }
            }
            model.IsDeleted = true;
            model.IsNew = model.IsModified = false;
            // Event auch feuern, wenn keine DB-Aktion erforderlich ist, damit sich das ViewModel dennoch aktualisieren kann
            OnModelDeleted?.Invoke(this, model);
        }

        protected virtual async Task InsertAsync(MoviekusDbContext context, T model)
        {
            try
            {
                await context.Set<T>().AddAsync(model);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }

        }

        protected virtual void UpdateAsync(MoviekusDbContext context, T model)
        {
            context.Entry(model).State = EntityState.Modified;
        }

        protected virtual void DeleteAsync(MoviekusDbContext context, T model)
        {
        }
    }
}
