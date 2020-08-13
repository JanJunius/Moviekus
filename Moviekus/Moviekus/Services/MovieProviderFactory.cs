using Moviekus.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviekus.Services
{
    public enum MovieProviders
    {
        MovieDb
    };

    public static class MovieProviderFactory
    {
        public static IMovieProvider CreateMovieProvider(MovieProviders provider)
        {
            switch(provider)
            {
                case MovieProviders.MovieDb: return MovieDbService.Ref;
                default: return null;
            }
        }
    }
}
