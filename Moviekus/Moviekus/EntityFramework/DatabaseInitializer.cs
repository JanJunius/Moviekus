using Microsoft.EntityFrameworkCore;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            genres.ForEach(g => g.IsNew = true);
            genres.ForEach(g => context.Add(g));
            await context.SaveChangesAsync();

            if (context.Sources.Count() == 0)
            {
                var sources = new List<Source>()
                {
                    new Source { Name = "Lokal", SourceTypeName = "Lokal"},
                    new Source { Name = "Netflix", SourceTypeName = "Netflix"},
                    new Source { Name = "Amazon Prime", SourceTypeName = "Amazon Prime"}
                };
                sources.ForEach(s => s.IsNew = true);
                sources.ForEach(s => context.Add(s));
                await context.SaveChangesAsync();
            }

            if (context.Settings.Count() == 0)
            {
                Settings settings = Settings.CreateNew<Settings>();

                settings.MovieDb_ApiKey = "121d3dae50d820edc0329e0bcf02c712";
                settings.MovieDb_Language = "de-DE";

                context.Add(settings);
                await context.SaveChangesAsync();
            }

            Filter filter = Filter.CreateNew<Filter>();
            filter.Name = "Filter 1";
            context.Filter.Add(filter);
            await context.SaveChangesAsync();

            FilterEntry entry1 = FilterEntry.CreateNew<FilterEntry>();
            entry1.Filter = filter;
            entry1.FilterEntryProperty = FilterEntryProperty.Title;
            entry1.ValueFrom = "Krieger";
            context.FilterEntries.Add(entry1);
            FilterEntry entry2 = FilterEntry.CreateNew<FilterEntry>();
            entry2.Filter = filter;
            entry2.FilterEntryProperty = FilterEntryProperty.LastSeen;
            entry2.ValueFrom = DateTime.MinValue.ToString();
            context.FilterEntries.Add(entry2);
            FilterEntry entry3 = FilterEntry.CreateNew<FilterEntry>();
            entry3.Filter = filter;
            entry3.FilterEntryProperty = FilterEntryProperty.Rating;
            entry3.ValueFrom = "4";
            context.FilterEntries.Add(entry3);
            FilterEntry entry4 = FilterEntry.CreateNew<FilterEntry>();
            entry4.Filter = filter;
            entry4.FilterEntryProperty = FilterEntryProperty.ReleaseDate;
            entry4.ValueFrom = DateTime.Today.AddMonths(-10).ToString();
            entry4.ValueTo = DateTime.Today.AddMonths(-4).ToString();
            context.FilterEntries.Add(entry4);
            await context.SaveChangesAsync();

            //var movies = new List<Movie>()
            //{
            //    new Movie { Title = "Mein erster Film von Netflix", Source = sources[1] },
            //    new Movie { Title = "Mein erster Film von Amazon", Source = sources[2], Description="Dies ist mein erster Film bei Amazon Prime, der aber nur zum Testen angelegt wurde.\r\nSpäter kommen echte Filme dazu", ReleaseDate=DateTime.Today.AddMonths(-24), Runtime=123, Rating=4 }
            //};
            //movies.ForEach(m => m.IsNew = true);
            //movies.ForEach(m => context.Add(m));
            //await context.SaveChangesAsync();

            //movies[0].MovieGenres = new List<MovieGenre>
            //{
            //    new MovieGenre() { Genre = genres[1], Movie = movies[0] }
            //};
            //await context.SaveChangesAsync();

            //movies[1].MovieGenres = new List<MovieGenre>
            //{
            //    new MovieGenre() { Genre = genres[1], Movie = movies[1] },
            //    new MovieGenre() { Genre = genres[2], Movie = movies[1] }
            //};
            //await context.SaveChangesAsync();

        }

        public static void RecreateDatabase(MoviekusDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

    }
}
