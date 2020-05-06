using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics;
using System.IO;

namespace Moviekus.EntityFramework
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MoviekusDbContext>
    {
        public MoviekusDbContext CreateDbContext(string[] args)
        {
            Debug.WriteLine(Directory.GetCurrentDirectory() + @"\Config.db");

            return new MoviekusDbContext(Directory.GetCurrentDirectory() + @"\Config.db");
        }
    }
}
