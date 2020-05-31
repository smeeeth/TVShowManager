using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor.API
{
    public class IMDBAPI : APIWrapper<IMDBMetaData>
    {
        public IMDBAPI(String authKey) : 
            base(authKey, "https://movie-database-imdb-alternative.p.rapidapi.com/?i={id}&r=json")
        {
        }

        public override IMDBMetaData GetMetadata(string id)
        {
            return new IMDBMetaData(APIService.SendRequest(id, "IMDB_ApiResponse.txt"));
        }
    }
}
