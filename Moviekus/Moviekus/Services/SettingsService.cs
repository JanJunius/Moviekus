using Moviekus.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class SettingsService : BaseService<Settings>
    {
        public Settings GetSettings()
        {
            var settings = Get();
            if (settings.Count() == 0)
            {
                Settings newSettings = Settings.CreateNew<Settings>();
                return SaveChanges(newSettings);
            }
            return settings.First();
        }

        public async Task<Settings> GetSettingsAsync()
        {
            var settings = await GetAsync();
            
            if (settings.Count() == 0)
            {
                Settings newSettings = Settings.CreateNew<Settings>();
                return SaveChanges(newSettings);
            }
            return settings.First();
        }
    }
}
