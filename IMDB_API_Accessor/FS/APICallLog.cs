using System;
using System.IO;
using RestSharp;
using APIAccessor.Data;

namespace APIAccessor.FS
{
    public class APICallLog<T> where T : APIMetaData
    {
        private string FileName;
        private string AuthKey;
        public int CallsThisMonth { get; private set; } = 0;

        public APICallLog(string logDirectory, string authKey)
        {
            FileName = Path.Combine(logDirectory, $"Log-{typeof(T).Name}.txt");
            AuthKey = authKey;

            if (File.Exists(FileName))
            {
                foreach (string logLine in File.ReadAllLines(FileName))
                {
                    if (IsThisMonth(logLine))
                    {
                        CallsThisMonth++;
                    }

                }
            }
        }

        public void Log(RestClient client, RestRequest request, IRestResponse response)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(FileName))
            {
                file.WriteLine($"{DateTime.Now.ToString(APILogConfig.LOG_DATE_TIME_FORMAT)},{request.Method.ToString()},{client.BaseUrl},{response.Content}");
                CallsThisMonth++;
            }
        }

        private static bool IsThisMonth(string logLine)
        {
            var timeStamp = DateTime.ParseExact(logLine.Split(",")[0], APILogConfig.LOG_DATE_TIME_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
            return timeStamp.Month == DateTime.Now.Month;
        }
    }
}
