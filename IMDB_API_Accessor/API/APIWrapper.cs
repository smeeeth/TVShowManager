using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor.API
{
    public abstract class APIWrapper<T> where T : APIMetaData
    {
        protected RapidAPI<T> APIService;
        public APIWrapper(string authKey, String baseUrl)
        {
            APIService = new RapidAPI<T>(authKey, baseUrl);
        }

        public abstract T GetMetadata(String id);


    }
}
