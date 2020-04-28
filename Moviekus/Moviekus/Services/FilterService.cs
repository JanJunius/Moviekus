using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
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

        protected override async Task InsertAsync(MoviekusDbContext context, Filter filter)
        {
            context.Entry(filter).State = EntityState.Added;

            foreach(var filterEntry in filter.FilterEntries)
            {
                if (context.Entry(filterEntry).State == EntityState.Detached)
                {
                    // Keine neu hinzugefügten und gleich wieder entfernten Einträge betrachten
                    if (!filterEntry.IsDeleted)
                        context.Entry(filterEntry).State = EntityState.Added;
                }

                filterEntry.IsNew = filterEntry.IsModified = filterEntry.IsDeleted = false;
            }

            await base.InsertAsync(context, filter);
        }

        protected override void UpdateAsync(MoviekusDbContext context, Filter filter)
        {
            base.UpdateAsync(context, filter);

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

    }
}
