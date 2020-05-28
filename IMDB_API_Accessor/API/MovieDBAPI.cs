using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor.API
{
    class MovieDBAPI : APIWrapper<MovieDBMetaData>
    {
        public MovieDBAPI(String authKey) :
            base(authKey, "https://movie-database-imdb-alternative.p.rapidapi.com/?i={0}&r=json")
        {
        }
        public override MovieDBMetaData GetMetadata(string id)
        {
            return new MovieDBMetaData(APIService.SendRequest(id, "MovieDB_ApiResponse.txt"));
        }
    }
}
