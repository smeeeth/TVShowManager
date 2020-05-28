using RestSharp;
using System;

namespace APIAccessor.API
{
    public class APIManager
    {
        public const bool IsTesting = true;

        private static string APIKEY = "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860";

        static private IMDBAPI IMDB_API = new IMDBAPI(APIKEY);
        static private MovieDBAPI MovieDB_API = new MovieDBAPI(APIKEY);

        public static IMDBMetaData GetByIDIMDB(string id)
        {
            return IMDB_API.GetMetadata(id);
        }

        public static MovieDBMetaData GetByIdMovieDB(string id)
        {
            return MovieDB_API.GetMetadata(id);
        }

        public static TVMetaData GetByID(string id)
        {
            IMDBMetaData imdb = GetByIDIMDB(id);
            MovieDBMetaData movieDB = GetByIdMovieDB(id);

            return new TVMetaData(imdb, movieDB);
        }
    }
}
