using Moviekus.EntityFramework;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class SourceService : BaseService<Source>
    {
        public static Source GetDefaultSource()
        {
            
            using (var context = new MoviekusDbContext())
            {
                return context.Sources.Where(s => s.SourceTypeName == SourceType.DefaultSourceType.Name).FirstOrDefault();
            }
        }
    }
}
