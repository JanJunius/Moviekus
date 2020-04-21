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

            var sources = new List<Source>();
            if (context.Sources.Count() == 0)
            {
                sources.Add(new Source { Name = "Lokal", SourceTypeName = "Lokal" });
                sources.Add(new Source { Name = "Netflix", SourceTypeName = "Netflix" });
                sources.Add(new Source { Name = "Amazon Prime", SourceTypeName = "Amazon Prime" });

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

            var filterEntryTypes = new List<FilterEntryType>()
            {
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Description, Name = "Beschreibung"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Genre, Name = "Genre"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.LastSeen, Name = "Zuletzt gesehen"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Notes, Name = "Bemerkung"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Rating, Name = "Bewertung"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.ReleaseDate, Name = "Veröffentlichungsdatum"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Runtime, Name = "Laufzeit"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Source, Name = "Verfügbar bei"},
                new FilterEntryType() { Id = Guid.NewGuid().ToString(), IsNew = true, Property = FilterEntryProperty.Title, Name = "Titel"}
            };
            filterEntryTypes.ForEach(f => context.Add(f));
            await context.SaveChangesAsync();

            Filter filter = Filter.CreateNew<Filter>();
            filter.Name = "Filter 1";
            context.Filter.Add(filter);
            await context.SaveChangesAsync();

            FilterEntry entry1 = FilterEntry.CreateNew<FilterEntry>();
            entry1.Filter = filter;
            entry1.FilterEntryType = filterEntryTypes[8];
            entry1.ValueFrom = "Krieger";
            context.FilterEntries.Add(entry1);
            FilterEntry entry2 = FilterEntry.CreateNew<FilterEntry>();
            entry2.Filter = filter;
            entry2.FilterEntryType = filterEntryTypes[2];
            entry2.ValueFrom = MoviekusDefines.MinDate.ToString("d");
            entry2.ValueTo = MoviekusDefines.MinDate.ToString("d");
            context.FilterEntries.Add(entry2);
            FilterEntry entry3 = FilterEntry.CreateNew<FilterEntry>();
            entry3.Filter = filter;
            entry3.FilterEntryType = filterEntryTypes[4];
            entry3.ValueFrom = "4";
            context.FilterEntries.Add(entry3);
            FilterEntry entry4 = FilterEntry.CreateNew<FilterEntry>();
            entry4.Filter = filter;
            entry4.FilterEntryType = filterEntryTypes[5];
            entry4.ValueFrom = DateTime.Today.AddMonths(-10).ToString("d");
            entry4.ValueTo = DateTime.Today.AddMonths(-4).ToString("d");
            context.FilterEntries.Add(entry4);
            FilterEntry entry5 = FilterEntry.CreateNew<FilterEntry>();
            entry5.Filter = filter;
            entry5.FilterEntryType = filterEntryTypes[7];
            entry5.ValueFrom = sources[1].Id;
            context.FilterEntries.Add(entry5);
            FilterEntry entry6 = FilterEntry.CreateNew<FilterEntry>();
            entry6.Filter = filter;
            entry6.FilterEntryType = filterEntryTypes[1];
            entry6.ValueFrom = genres[1].Id;
            context.FilterEntries.Add(entry6);
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
