using Nancy.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace APIAccessor
{
    public class Program
    {
        static void Main(string[] args)
        {
            var id = "tt0903747";

            //var textIMDB = IMDBSendRequest(id);
            var textIMDB = System.IO.File.ReadAllText("IMDB_ApiResponse.txt");

            //System.IO.File.WriteAllText("IMDB_ApiResponse.txt", textIMDB);

            IMDBMetaData imdbDBData = new IMDBMetaData(textIMDB);


            //var textMovieDB = MovieDBRequest(id);
            var textMovieDB = System.IO.File.ReadAllText("MovieDB_ApiResponse.txt");

            //System.IO.File.WriteAllText($"C:/Users/erics/Workspace/IMDB_API_Accessor/IMDB_API_Accessor/MovieDB_ApiResponse.txt", textMovieDB);

            MovieDBMetaData movieDBData = new MovieDBMetaData(textMovieDB);

            TVMetaData metaData = GetByID(id);

            Console.ReadLine();
        }

        static List<IMDBMetaData> GetManyByIds(List<string> ids)
        {
            List<IMDBMetaData> metas = new List<IMDBMetaData>();
            foreach (string id in ids)
            {
                metas.Add(GetByIDIMDB(id));
            }
            return metas;
        }

        public static TVMetaData GetByID(string id)
        {
            IMDBMetaData imdb = GetByIDIMDB(id);
            MovieDBMetaData movieDB = GetByIdMovieDB(id);

            return new TVMetaData(imdb, movieDB);
        }

        static string IMDBSendRequest(string id)
        {
            Console.WriteLine("Sending Request...");
            var client = new RestClient($"https://imdb8.p.rapidapi.com/title/get-meta-data?region=US&ids={id}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860");
            //var text = client.Execute(request).Content;
            var text = System.IO.File.ReadAllText("IMDB_ApiResponse.txt");
            return text;
        }

        static IMDBMetaData GetByIDIMDB(string id)
        {
            return new IMDBMetaData(IMDBSendRequest(id));
        }

        static string MovieDBRequest(string id)
        {
            var client = new RestClient($"https://movie-database-imdb-alternative.p.rapidapi.com/?i={id}&r=json");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860");
            //var text = client.Execute(request).Content;
            var text = System.IO.File.ReadAllText("MovieDB_ApiResponse.txt");
            return text;
        }

        static MovieDBMetaData GetByIdMovieDB(string id)
        {
            return new MovieDBMetaData(MovieDBRequest(id));
        }
    }
        
}
