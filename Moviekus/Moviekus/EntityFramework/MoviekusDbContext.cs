using Microsoft.EntityFrameworkCore;
using Moviekus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moviekus.EntityFramework
{
    public class MoviekusDbContext : DbContext
    {
        // Für jede von EF zu verwaltende Entity ist hier ein DbSet anzulegen

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<Filter> Filter { get; set; }
        public virtual DbSet<FilterEntry> FilterEntries{ get; set; }
        public virtual DbSet<FilterEntryType> FilterEntryTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string databasePath = Path.Combine(documentPath, MoviekusDefines.DbFileName);
                // Specify that we will use sqlite and the path of the database here
                optionsBuilder.UseSqlite($"Filename={databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
