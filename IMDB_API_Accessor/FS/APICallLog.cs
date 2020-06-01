using System;
using System.IO;
using RestSharp;

namespace APIAccessor.FS
{
    class APICallLog
    {
        private string FileName;
        private string AuthKey;
        public APICallLog(string fileName, string authKey)
        {
            FileName = fileName;
            AuthKey = authKey;
        }

        public void Log(RestClient client, RestRequest request, IRestResponse response)
        {
           File.WriteAllText(FileName, $"{DateTime.Now.ToString()} {request.Method.ToString()} {client.BaseUrl} {response.Content}");
        }
    }
}
