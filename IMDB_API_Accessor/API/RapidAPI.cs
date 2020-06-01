using System;
using RestSharp;
using APIAccessor.FS;

namespace APIAccessor.API
{
    public class RapidAPI
    {
        //Authentication key
        private string AuthKey;

        //Formatted string of base address
        private string BaseAddress;

        //Logger
        private static APICallLog Logger;

        public RapidAPI(string authKey, string baseAddress)
        {
            AuthKey = authKey;
            BaseAddress = baseAddress;

            Logger = new APICallLog("", authKey);
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
