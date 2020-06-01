using System;
using APIAccessor.Data;
using APIAccessor.FS;

namespace APIAccessor.API
{
    public abstract class APIWrapper<T> where T : APIMetaData
    {
        protected RapidAPI<T> APIService;
        protected APIWrapper(string authKey, String baseUrl, APICallLog<T> logger)
        {
            APIService = new RapidAPI<T>(authKey, baseUrl, logger);
        }

        public abstract T GetMetadata(String id);


    }
}
