using System;
using RestSharp;
using APIAccessor.Data;
using APIAccessor.FS;

namespace APIAccessor.API
{
    public class RapidAPI<T> where T : APIMetaData
    {
        //Authentication key
        private string AuthKey;

        //Formatted string of base address
        private string BaseAddress;

        //Logger
        public APICallLog<T> Logger { get; set; }

        public RapidAPI(string authKey, string baseAddress, APICallLog<T> logger)
        {
            AuthKey = authKey;
            BaseAddress = baseAddress;
            Logger = logger;
        }

        public string SendRequest(string id, string testFile = null) {
            var client = new RestClient(string.Format(BaseAddress, id));
            var request = new RestRequest(Method.GET);
            var headerPretext = new Uri(BaseAddress).Host;
            request.AddHeader("x-rapidapi-host", headerPretext);
            request.AddHeader("x-rapidapi-key", $"{AuthKey}");

            string text = "";
            if (!APIManager.IsTesting)
            {
                var response = client.Execute(request);
                Logger.Log(client, request, response); 
                text = response.Content;
            }
            else if (testFile != null)
            {
                text = System.IO.File.ReadAllText(testFile);
            }

            return text;
        }

    }
}
