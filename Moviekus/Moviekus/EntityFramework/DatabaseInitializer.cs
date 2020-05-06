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
        }

        public static void RecreateDatabase(MoviekusDbContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

    }
}
