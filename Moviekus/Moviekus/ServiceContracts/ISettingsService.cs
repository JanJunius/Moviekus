using Moviekus.Models;
using System.Threading.Tasks;

namespace Moviekus.ServiceContracts
{
    public interface ISettingsService : IBaseService<Settings>
    {
        Settings GetSettings();

        Task<Settings> GetSettingsAsync();
    }
}
