using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor
{
    public class TVMetaData
    {
        public TVMetaData(IMDBMetaData imdb, MovieDBMetaData movieDB)
        {
            Id = imdb.Id;
            Title = imdb.Title;
            StartYear = imdb.StartYear;
            EndYear = imdb.EndYear;
            NumEpisodes = imdb.NumEpisodes;
            AverageRunningTimeMinutes = imdb.AverageRunningTimeMinutes;
            Rating = imdb.Rating;
            RatingCount = imdb.RatingCount;
            WayToWatch = imdb.WayToWatch;
            Genres = imdb.Genres;
            Certificate = imdb.Certificate;

            Released = movieDB.Released;
            Director = movieDB.Director;
            Actors = movieDB.Actors;
            Plot = movieDB.Plot;
            Language = movieDB.Language;
            Country = movieDB.Country;
            Awards = movieDB.Awards;
            TotalSeasons = movieDB.TotalSeasons;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int NumEpisodes { get; set; }
        public int AverageRunningTimeMinutes { get; set; }
        public decimal Rating { get; set; }
        public long RatingCount { get; set; }
        public WaysToWatch WayToWatch { get; set; }
        public string[] Genres { get; set; }
        public string Certificate { get; set; }
        public string Released { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public int TotalSeasons { get; set; }
    }
}
