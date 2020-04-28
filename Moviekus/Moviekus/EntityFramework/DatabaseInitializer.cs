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
            if (context.Genres.Count() == 0)
            {
                var genres = new List<Genre>()
                {
                    new Genre { Id = "dca20c06-5b6e-4a43-a850-ac5343ecc465", Name = "Musik" },
                    new Genre { Id = "47a65cb0-7f35-490d-9749-2bd923939438", Name = "Action" },
                    new Genre { Id = "6ab67134-55f4-4c39-9e5e-704606b82460", Name = "Komödie" },
                    new Genre { Id = "de0fe707-128f-47db-be0b-63815bc61a64", Name = "Drama" }
                };
                genres.ForEach(g => g.IsNew = true);
                genres.ForEach(g => context.Add(g));
                await context.SaveChangesAsync();
            }

            if (context.Sources.Count() == 0)
            {
                var sources = new List<Source>();
                sources.Add(new Source { Id = "4f72aa31-38b3-48b6-988e-3f96df94c44f", Name = "Lokal", SourceTypeName = "Lokal" });
                sources.Add(new Source { Id = "79aab413-5c1b-4446-9288-72f0f476b219", Name = "Netflix", SourceTypeName = "Netflix" });
                sources.Add(new Source { Id = "3fe0f150-c349-4462-99fd-0e61e6e513a7", Name = "Amazon Prime", SourceTypeName = "Amazon Prime" });

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

            if (context.FilterEntryTypes.Count() == 0)
            {
                var filterEntryTypes = new List<FilterEntryType>()
            {
                new FilterEntryType() { Id = "d33a4909-2f26-499f-a4b1-25b09a29bf31", IsNew = true, Property = FilterEntryProperty.Description, Name = "Beschreibung"},
                new FilterEntryType() { Id = "2829099a-dc9d-4a6c-9e1a-13bb4bc86732", IsNew = true, Property = FilterEntryProperty.Genre, Name = "Genre"},
                new FilterEntryType() { Id = "ec6e8e1d-1b13-4914-b99e-d4daae8ff4d9", IsNew = true, Property = FilterEntryProperty.LastSeen, Name = "Zuletzt gesehen"},
                new FilterEntryType() { Id = "f37872db-a61f-40e8-a3e5-6c46a74a2b77", IsNew = true, Property = FilterEntryProperty.Remarks, Name = "Bemerkung"},
                new FilterEntryType() { Id = "eb5685d5-03e4-4a5a-ae27-6bd17ef35d8c", IsNew = true, Property = FilterEntryProperty.Rating, Name = "Bewertung"},
                new FilterEntryType() { Id = "86d7f54b-0730-4a01-9ff7-4f9fb0cb55c4", IsNew = true, Property = FilterEntryProperty.ReleaseDate, Name = "Veröffentlichungsdatum"},
                new FilterEntryType() { Id = "1821f370-6953-4f9a-bdaf-e0be7fb1bea6", IsNew = true, Property = FilterEntryProperty.Runtime, Name = "Laufzeit"},
                new FilterEntryType() { Id = "2117437b-9895-4c66-bc79-1e4dd025d125", IsNew = true, Property = FilterEntryProperty.Source, Name = "Verfügbar bei"},
                new FilterEntryType() { Id = "1bee1bbb-a1c8-4a45-b599-19fb2deadc96", IsNew = true, Property = FilterEntryProperty.Title, Name = "Titel"}
            };
                filterEntryTypes.ForEach(f => context.Add(f));
                await context.SaveChangesAsync();
            }

            /*
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
            */

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
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

    }
}
