using RestSharp;
using System;
using System.IO;
using System.Linq;

namespace APIAccessor.FS
{
    class APILogger
    {
        private string FileName;

        public APILogger(String apiName)
        {
            FileName = $"{apiName}Log.log";
        }
        public void Log(RestClient client, RestRequest request, IRestResponse response)
        {
            var logLine = $"{DateTime.Now.ToString()}, {request.Method}, {client.BaseUrl.ToString()}, " +
                $"{response.Headers.ToList().Find(x => x.Name == "X-RateLimit-Requests-Limit").Value.ToString()}, {response.Headers.ToList().Find(x => x.Name == "X-RateLimit-Requests-Remaining").Value.ToString()}, {response.StatusCode}, {response.Content}";
            File.AppendAllText(FileName, logLine + Environment.NewLine);
        }
    }
}
