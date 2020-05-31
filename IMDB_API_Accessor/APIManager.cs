using RestSharp;
using System;

namespace APIAccessor
{
    public static class APIManager
    {
        const bool IsTesting = true;

        private static string APIKEY = "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860";

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
            IMDBMetaData imdb = GetByIDIMDB(id);
            MovieDBMetaData movieDB = GetByIdMovieDB(id);

            return new TVMetaData(imdb, movieDB);
        }
    }
}
