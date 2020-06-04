using APIAccessor.API;
using APIAccessor.Data;
using RestSharp;
using System;

namespace APIAccessor
{
    public static class APIManager
    {
        public const bool IsTesting = false;

        private static string APIKEY = "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860";

        private static IMDBAPI ImdbApi = new IMDBAPI(APIKEY);
        private static MovieDBAPI MovieDbApi = new MovieDBAPI(APIKEY);

        public static IMDBMetaData GetByIDIMDB(string id)
        {
            return new IMDBMetaData(IMDBSendRequest(id));
        }

        static string IMDBSendRequest(string id)
        {
            Console.WriteLine("Sending Request...");
            var client = new RestClient($"https://imdb8.p.rapidapi.com/title/get-meta-data?region=US&ids={id}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", $"{APIKEY}");

            string text;
            if (!IsTesting)
            { 
                text = client.Execute(request).Content;
            } else
            {
                text = System.IO.File.ReadAllText("IMDB_ApiResponse.txt");
            }
            
            return text;
        }

        public static MovieDBMetaData GetByIdMovieDB(string id)
        {
            return new MovieDBMetaData(MovieDBRequest(id));
        }

        static string MovieDBRequest(string id)
        {
            var client = new RestClient($"https://movie-database-imdb-alternative.p.rapidapi.com/?i={id}&r=json");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", $"{APIKEY}");

            string text;
            if (!IsTesting)
            {
                text = client.Execute(request).Content;
            }
            else
            {
                text = System.IO.File.ReadAllText("MovieDB_ApiResponse.txt");
            }
            return text;
        }

        public static TVMetaData GetByID(string id)
        {
            IMDBMetaData imdb = ImdbApi.GetMetadata(id);
            MovieDBMetaData movieDB = MovieDbApi.GetMetadata(id);

            return new TVMetaData(imdb, movieDB);
        }


        public static event EventHandler<MyEventArgs> RequestMade = delegate { };

        internal static void NotifyCallMade<T>(RapidAPI<T> apiCalledTo) where T : APIMetaData
        {
            RequestMade(apiCalledTo, new MyEventArgs(apiCalledTo.ApiName, apiCalledTo.MaxCallsPerMonth, apiCalledTo.CallsRemaining));
        }
    }

    //Define event argument you want to send while raising event.
    public class MyEventArgs : EventArgs
    {
        public string APIName { get; set; }
        public int MaxCalls { get; set; }
        public int CallsRemaining { get; set; }


        public MyEventArgs(string apiName, int maxCalls, int callsRemaining)
        {
            APIName = apiName;
            MaxCalls = maxCalls;
            CallsRemaining = callsRemaining;
        }
    }


}
