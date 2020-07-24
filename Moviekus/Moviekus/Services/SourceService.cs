using Moviekus.Models;
using Moviekus.ServiceContracts;

namespace Moviekus.Services
{
    public class SourceService : BaseService<Source>, ISourceService
    {
        public Source CreateSource()
        {
            return Source.CreateNew<Source>();
        }
    }
}
