using Microsoft.EntityFrameworkCore;
using Moviekus.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moviekus.EntityFramework
{
    public class MoviekusDbContext : DbContext
    {
        private string _databasePath;

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<Filter> Filter { get; set; }
        public virtual DbSet<FilterEntry> FilterEntries{ get; set; }
        public virtual DbSet<FilterEntryType> FilterEntryTypes { get; set; }

        public MoviekusDbContext()
        {
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _databasePath = Path.Combine(documentPath, MoviekusDefines.DbFileName);
        }

        public MoviekusDbContext(string databasePath)
        {
            _databasePath = databasePath;
            Migrate();
        }

        public void Migrate()
        {
			LogManager.GetCurrentClassLogger().Info("Starting Migration of database...");
            Database.Migrate();
            LogManager.GetCurrentClassLogger().Info("Finished Migration of database.");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Specify that we will use sqlite and the path of the database here
                optionsBuilder.UseSqlite($"Filename={_databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
