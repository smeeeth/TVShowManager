using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor.API
{
    public class API<T> where T : APIMetaData
    {
        private static bool IsTesting = true;

        //Authentication key
        private string AuthKey;

        //Formatted string of base address
        private string BaseAddress;

        public API(string authKey, string baseAddress)
        {
            AuthKey = authKey;
            BaseAddress = baseAddress;
        }

        public string SendRequest(string id, string testFile = null) {
            Console.WriteLine("Sending Request...");
            var client = new RestClient(string.Format(BaseAddress, id));
            var request = new RestRequest(Method.GET);
            var headerPretext = new Uri(BaseAddress).Host;
            request.AddHeader("x-rapidapi-host", headerPretext);
            request.AddHeader("x-rapidapi-key", $"{AuthKey}");

            string text = "";
            if (!IsTesting)
            {
                text = client.Execute(request).Content;
            }
            else if (testFile == null)
            {
                text = System.IO.File.ReadAllText(testFile);
            }

            return text;
        }

    }
}
