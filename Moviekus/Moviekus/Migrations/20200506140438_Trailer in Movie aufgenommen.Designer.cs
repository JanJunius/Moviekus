﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moviekus.EntityFramework;

namespace Moviekus.Migrations
{
    [DbContext(typeof(MoviekusDbContext))]
    [Migration("20200506140438_Trailer in Movie aufgenommen")]
    partial class TrailerinMovieaufgenommen
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Moviekus.Models.Filter", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Filter");
                });

            modelBuilder.Entity("Moviekus.Models.FilterEntry", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FilterEntryTypeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FilterId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ValueFrom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ValueTo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FilterEntryTypeId");

                    b.HasIndex("FilterId");

                    b.ToTable("FilterEntries");
                });

            modelBuilder.Entity("Moviekus.Models.FilterEntryType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Property")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("FilterEntryTypes");
                });

            modelBuilder.Entity("Moviekus.Models.Genre", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Moviekus.Models.Movie", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Cover")
                        .HasColumnType("BLOB");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Homepage")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .HasColumnType("TEXT");

                    b.Property<int>("Runtime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Trailer")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Moviekus.Models.MovieGenre", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("GenreId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MovieId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieGenres");
                });

            modelBuilder.Entity("Moviekus.Models.Settings", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("MovieDb_ApiKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("MovieDb_Language")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Moviekus.Models.Source", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceTypeName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("Moviekus.Models.FilterEntry", b =>
                {
                    b.HasOne("Moviekus.Models.FilterEntryType", "FilterEntryType")
                        .WithMany()
                        .HasForeignKey("FilterEntryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moviekus.Models.Filter", "Filter")
                        .WithMany("FilterEntries")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Moviekus.Models.Movie", b =>
                {
                    b.HasOne("Moviekus.Models.Source", "Source")
                        .WithMany("Movies")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Moviekus.Models.MovieGenre", b =>
                {
                    b.HasOne("Moviekus.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moviekus.Models.Movie", "Movie")
                        .WithMany("MovieGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}