using Microsoft.EntityFrameworkCore;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.EntityFramework
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeData(MoviekusDbContext context)
        {
            var genres = new List<Genre>()
            {
                new Genre { Name = "Musik" },
                new Genre { Name = "Action" },
                new Genre { Name = "Komödie" },
                new Genre { Name = "Drama" }
            };

            genres.ForEach(g => context.Add(g));
            await context.SaveChangesAsync();

            var sources = new List<Source>()
            {
                new Source { Name = "Lokal", SourceTypeName = "Lokal"},
                new Source { Name = "Netflix", SourceTypeName = "Netflix"},
                new Source { Name = "Amazon Prime", SourceTypeName = "Amazon Prime"}
            };

            sources.ForEach(s => context.Add(s));
            await context.SaveChangesAsync();
        }

        public static void RecreateDatabase(MoviekusDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

    }
}
