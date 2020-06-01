using System;
using APIAccessor.Data;

namespace APIAccessor.API
{
    public abstract class APIWrapper<T> where T : APIMetaData
    {
        protected RapidAPI APIService;
        public APIWrapper(string authKey, String baseUrl)
        {
            APIService = new RapidAPI(authKey, baseUrl);
        }

        public abstract T GetMetadata(String id);


    }
}
