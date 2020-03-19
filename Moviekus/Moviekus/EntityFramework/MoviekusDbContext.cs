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
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Source> Sources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string databasePath = Path.Combine(documentPath, "Moviekus.db3");
                // Specify that we will use sqlite and the path of the database here
                optionsBuilder.UseSqlite($"Filename={databasePath}");
            }

        }
    }
}
