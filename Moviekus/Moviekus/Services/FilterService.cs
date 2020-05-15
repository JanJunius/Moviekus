using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class FilterService : BaseService<Filter>
    {
        public override async Task<IEnumerable<Filter>> GetAsync()
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.Filter.Include(f => f.FilterEntries).ThenInclude(t => t.FilterEntryType).ToListAsync();
            }
        }

        public async Task<IEnumerable<FilterEntryType>> GetFilterEntryTypesAsync()
        {
            using (var context = new MoviekusDbContext())
            {
                return await context.FilterEntryTypes.ToListAsync();
            }
        }

        public Filter GetDefault()
        {
            using (var context = new MoviekusDbContext())
            {
                return context.Filter.Where(f => f.IsDefault == true).Include(fe => fe.FilterEntries).ThenInclude(fet => fet.FilterEntryType).FirstOrDefault();
            }
        }

        public async Task<Filter> SetDefault(Filter filter)
        {
            try
            {
                using (var context = new MoviekusDbContext())
                {
                    await ResetDefault();
                    filter.IsDefault = true;
                    return await SaveChangesAsync(filter);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            return filter;
        }

        public async Task ResetDefault()
        {
            try
            {
                using (var context = new MoviekusDbContext())
                {
                    var defaultFilter = GetDefault();
                    if (defaultFilter != null)
                    {
                        defaultFilter.IsDefault = false;
                        await SaveChangesAsync(defaultFilter);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }

        protected override async Task InsertAsync(MoviekusDbContext context, Filter filter)
        {
            try
            {
                context.Entry(filter).State = EntityState.Added;

                foreach (var filterEntry in filter.FilterEntries)
                {
                    if (context.Entry(filterEntry).State == EntityState.Detached)
                    {
                        // Keine neu hinzugefügten und gleich wieder entfernten Einträge betrachten
                        if (!filterEntry.IsDeleted)
                            context.Entry(filterEntry).State = EntityState.Added;
                    }

                    filterEntry.IsNew = filterEntry.IsModified = filterEntry.IsDeleted = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
            await base.InsertAsync(context, filter);
        }

        protected override void UpdateAsync(MoviekusDbContext context, Filter filter)
        {
            base.UpdateAsync(context, filter);

            try
            {
                foreach (var filterEntry in filter.FilterEntries)
                {
                    if (filterEntry.IsNew)
                        context.Entry(filterEntry).State = EntityState.Added;
                    else if (filterEntry.IsDeleted)
                        context.Entry(filterEntry).State = EntityState.Deleted;
                    else if (filterEntry.IsModified)
                        context.Entry(filterEntry).State = EntityState.Modified;
                    else context.Entry(filterEntry).State = EntityState.Unchanged;

                    // Auf FilterEntryTypes wird nur referenziert, sie werden nie geändert
                    if (context.Entry(filterEntry.FilterEntryType).State != EntityState.Detached)
                        context.Entry(filterEntry.FilterEntryType).State = EntityState.Detached;

                    filterEntry.IsNew = filterEntry.IsModified = filterEntry.IsDeleted = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw;
            }
        }

        protected override void DeleteAsync(MoviekusDbContext context, Filter filter)
        {
            foreach(var filterEntry in filter.FilterEntries)
            {
                // Keine Entries löschen, die noch nie gespeichert wurden
                if (filterEntry.IsNew && context.Entry(filterEntry).State == EntityState.Deleted)
                    context.Entry(filterEntry).State = EntityState.Detached;
            }
        }
    }
}
