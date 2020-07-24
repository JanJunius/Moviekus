using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface IFilterService : IBaseService<Filter>
    {
        new Task<IList<Filter>> GetAsync();

        Task<IEnumerable<FilterEntryType>> GetFilterEntryTypesAsync();

        Filter GetDefault();

        Task<Filter> SetDefault(Filter filter);

        Task ResetDefault();
   }
}
