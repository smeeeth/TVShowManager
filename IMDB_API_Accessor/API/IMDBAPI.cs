using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor.API
{
    public class IMDBAPI : APIWrapper<IMDBMetaData>
    {
        public IMDBAPI(String authKey) : 
            base(authKey, "https://imdb8.p.rapidapi.com/title/get-meta-data?region=US&ids={0}")
        {
        }

        public override IMDBMetaData GetMetadata(string id)
        {
            return new IMDBMetaData(APIService.SendRequest(id, "IMDB_ApiResponse.txt"));
        }
    }
}
