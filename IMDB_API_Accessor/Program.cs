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

            TVMetaData metaData = API.APIManager.GetByID(id);

            Console.ReadLine();
        }
    }
        
}
