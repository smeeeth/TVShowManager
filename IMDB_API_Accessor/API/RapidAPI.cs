using System;
using RestSharp;
using APIAccessor.Data;
using System.Linq;

namespace APIAccessor.API
{
    public class RapidAPI<T> where T : APIMetaData
    {
        //Authentication key
        private string AuthKey;

        //Formatted string of base address
        private string BaseAddress;

        public int MaxCallsPerMonth { get; private set; }
        public int CallsThisMonth { get; private set; }

        public RapidAPI(string authKey, string baseAddress)
        {
            AuthKey = authKey;
            BaseAddress = baseAddress;
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

                MaxCallsPerMonth = Int32.Parse(response.Headers.ToList().Find(x => x.Name == "x-ratelimit-requests-limit").Value.ToString());
                CallsThisMonth = Int32.Parse(response.Headers.ToList().Find(x => x.Name == "x-ratelimit-requests-remaining").Value.ToString());
            }
            else if (testFile != null)
            {
                text = System.IO.File.ReadAllText(testFile);
            }

            return text;
        }

    }
}
