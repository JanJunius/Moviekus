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

        protected override void UpdateAsync(MoviekusDbContext context, Filter filter)
        {
            base.UpdateAsync(context, filter);

            foreach (var filterEntry in filter.FilterEntries)
            {
                if (context.Entry(filterEntry).State == EntityState.Detached)
                    context.Attach(filterEntry);

                if (filterEntry.IsNew)
                    context.Entry(filterEntry).State = EntityState.Added;
                if (filterEntry.IsModified)
                    context.Entry(filterEntry).State = EntityState.Modified;
                if (filterEntry.IsDeleted)
                    context.Entry(filterEntry).State = EntityState.Deleted;

                filterEntry.IsNew = filterEntry.IsModified = filterEntry.IsDeleted = false;
            }

        }

    }
}
