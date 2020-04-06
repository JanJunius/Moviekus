using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using System;
using System.Collections.Generic;
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
                return await context.Filter.Include(f => f.FilterEntries).ToListAsync();
            }
        }
    }
}
