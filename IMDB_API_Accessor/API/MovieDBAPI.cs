﻿using System;
using APIAccessor.Data;
using APIAccessor.FS;

namespace APIAccessor.API {
    public class MovieDBAPI : APIWrapper<MovieDBMetaData>, UsesIMDBId
    {
        public MovieDBAPI(String authKey) :
            base(authKey, "https://movie-database-imdb-alternative.p.rapidapi.com/?i={0}&r=json", new APICallLog<MovieDBMetaData>(APILogConfig.DEFAULT_LOG_DIRECTORY, authKey))
        {
        }

        public override MovieDBMetaData GetMetadata(string id)
        {
            return new MovieDBMetaData(APIService.SendRequest(id, "MovieDB_ApiResponse.txt"));
        }
    }
}
