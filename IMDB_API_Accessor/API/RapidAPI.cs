using System;
using RestSharp;
using APIAccessor.Data;
using System.Linq;
using APIAccessor.FS;

namespace APIAccessor.API
{
    public class RapidAPI<T> where T : APIMetaData
    {
        //Authentication key
        private string AuthKey;

        //Formatted string of base address
        private string BaseAddress;

        public string ApiName { get; private set; }

        private APILogger Logger;

        public int MaxCallsPerMonth { get; private set; }
        public int CallsRemaining { get; private set; }

        public RapidAPI(string authKey, string baseAddress, string apiName)
        {
            AuthKey = authKey;
            BaseAddress = baseAddress;
            ApiName = apiName;
            Logger = new APILogger(apiName);
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
                text = response.Content;
                Logger.Log(client, request, response);

                MaxCallsPerMonth = Int32.Parse(response.Headers.ToList().Find(x => x.Name == "X-RateLimit-Requests-Limit").Value.ToString());
                CallsRemaining = Int32.Parse(response.Headers.ToList().Find(x => x.Name == "X-RateLimit-Requests-Remaining").Value.ToString());
            }
            else if (testFile != null)
            {
                text = System.IO.File.ReadAllText(testFile);
                MaxCallsPerMonth = 500;
                CallsRemaining = 400;
            }

            APIManager.NotifyCallMade<T>(this);

            return text;
        }

    }
}
