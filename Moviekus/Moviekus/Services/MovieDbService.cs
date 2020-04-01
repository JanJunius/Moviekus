using Moviekus.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.Services
{
    public class MovieDbService
    {
        public IEnumerable<MovieDto> SearchMovie(string title)
        {
            List<MovieDto> movies = new List<MovieDto>()
            {
            new MovieDto()
            {
                Title = "Mein erster MovieDb-Film",
                ImDbId = "42",
                MovieDbId = "4242",
                Overview = "Dies ist nur ein Fake-Film, um die Schnittstelle zu MovieDb zu simulieren",
                ReleaseDate = DateTime.Today.AddMonths(-6),
                Runtime = 96,
                Genres = new List<string>() { "Action", "Neues Genre" }
            },
            new MovieDto()
            {
                Title = "Mein zweiter MovieDb-Film",
                ImDbId = "43",
                MovieDbId = "4343",
                Overview = "Dies ist nur ein weiterer Fake-Film, um die Schnittstelle zu MovieDb zu simulieren",
                ReleaseDate = DateTime.Today.AddMonths(-7),
                Runtime = 45,
                Genres = new List<string>() { "Action" }
            } };

            return movies;
        }
        
    }
}
