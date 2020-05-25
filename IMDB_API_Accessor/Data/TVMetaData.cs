using System.ComponentModel;

namespace APIAccessor
{
    public class TVMetaData : INotifyPropertyChanged
    {
        private TVMetaData()
        {

        }
        public TVMetaData(IMDBMetaData imdb, MovieDBMetaData movieDB, bool watched = false)
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

            Watched = watched;
        }

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { 
                _Id = value;
                RaisePropertyChangedEvent("Id");
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChangedEvent("Title");
            }
        }

        private int _StartYear;
        public int StartYear
        {
            get { return _StartYear; }
            set
            {
                _StartYear = value;
                RaisePropertyChangedEvent("StartYear");
            }
        }

        private int _EndYear;
        public int EndYear
        {
            get { return _EndYear; }
            set
            {
                _EndYear = value;
                RaisePropertyChangedEvent("EndYear");
            }
        }

        private int _NumEpisodes;
        public int NumEpisodes
        {
            get { return _NumEpisodes; }
            set
            {
                _NumEpisodes = value;
                RaisePropertyChangedEvent("NumEpisodes");
            }
        }

        private int _AverageRunningTimeMinutes;
        public int AverageRunningTimeMinutes
        {
            get { return _AverageRunningTimeMinutes; }
            set
            {
                _AverageRunningTimeMinutes = value;
                RaisePropertyChangedEvent("AverageRunningTimeMinutes");
            }
        }

        private decimal _Rating;
        public decimal Rating
        {
            get { return _Rating; }
            set 
            {
                _Rating = value;
                RaisePropertyChangedEvent("Rating");
            }
        }

        private long _RatingCount;
        public long RatingCount
        {
            get { return _RatingCount; }
            set
            {
                _RatingCount = value;
                RaisePropertyChangedEvent("RatingCount");
            }
        }

        private WaysToWatch _WayToWatch;
        public WaysToWatch WayToWatch {
            get{ return _WayToWatch; }
            set
            {
                _WayToWatch = value;
                RaisePropertyChangedEvent("WayToWatch");
            }
        }

        private string[] _Genres;
        public string[] Genres
        {
            get { return _Genres; }
            set
            {
                _Genres = value;
                RaisePropertyChangedEvent("Genres");
            }
        }

        private string _Certificate;
        public string Certificate
        {
            get { return _Certificate; }
            set
            {
                _Certificate = value;
                RaisePropertyChangedEvent("Certificate");
            }
        }

        private string _Released;
        public string Released
        {
            get { return _Released; }
            set
            {
                _Released = value;
                RaisePropertyChangedEvent("Released");
            }
        }

        private string _Director;
        public string Director
        {
            get { return _Director; }
            set
            {
                _Director = value;
                RaisePropertyChangedEvent("Director");
            }
        }

        private string _Actors;
        public string Actors
        {
            get { return _Actors; }
            set
            {
                _Actors = value;
                RaisePropertyChangedEvent("Actors");
            }
        }

        private string _Plot;
        public string Plot
        {
            get { return _Plot; }
            set
            {
                _Plot = value;
                RaisePropertyChangedEvent("Plot");
            }
        }

        private string _Language;
        public string Language
        {
            get { return _Language; }
            set
            {
                _Language = value;
                RaisePropertyChangedEvent("Language");
            }
        }

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set
            {
                _Country = value;
                RaisePropertyChangedEvent("Country");
            }
        }

        private string _Awards;
        public string Awards
        {
            get { return _Awards; }
            set
            {
                _Awards = value;
                RaisePropertyChangedEvent("Awards");
            }
        }

        private int _TotalSeasons;
        public int TotalSeasons
        {
            get { return _TotalSeasons; }
            set
            {
                _TotalSeasons = value;
                RaisePropertyChangedEvent("TotalSeasons");
            }
        }


        private bool _Watched;
        public bool Watched
        {
            get { return _Watched; }
            set
            {
                RaisePropertyChangedEvent("Watched");
                _Watched = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
    }
}
