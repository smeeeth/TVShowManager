using System;
using APIAccessor.Data;
using APIAccessor.FS;

namespace APIAccessor.API
{
    public abstract class APIWrapper<T> where T : APIMetaData
    {
        protected RapidAPI<T> APIService;

        protected APIWrapper(string authKey, String baseUrl, string name)
        {
            APIService = new RapidAPI<T>(authKey, baseUrl, name);
        }

        public abstract T GetMetadata(String id);


    }
}
