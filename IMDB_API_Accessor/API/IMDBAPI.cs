using System;
using APIAccessor.Data;
using APIAccessor.FS;

namespace APIAccessor.API
{
    public interface UsesIMDBId
    {

    }
    public class IMDBAPI : APIWrapper<IMDBMetaData>, UsesIMDBId
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
