using Nancy.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace IMDB_API_Accessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("API Accessor starting...");
            var client = new RestClient("https://imdb8.p.rapidapi.com/title/get-meta-data?region=US&ids=tt0903747");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860");

            //var response = client.Execute(request);
            //var text = response.Content;

            var text = System.IO.File.ReadAllText("C:/Users/erics/Workspace/IMDB_API_Accessor/IMDB_API_Accessor/TextFile302.txt");

            var num = DateTime.Now.Millisecond;
            System.IO.File.WriteAllText($"C:/Users/erics/Workspace/IMDB_API_Accessor/IMDB_API_Accessor/TextFile{num}.txt", text);

            JSONInterpreter interpreter = new JSONInterpreter();

            MetaData meta = JSONInterpreter.ReadMetaData(text);

            Console.WriteLine(meta);

            Console.ReadLine();

            MetaDataProcessor processor = new MetaDataProcessor();
            processor.ProcessMetaDatas(new List<MetaData> { meta });

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    Console.Write(processor.MainTable[i, j] + "\t");
                }
                Console.WriteLine();
            }


            Console.ReadLine();
        }

        List<MetaData> GetManyByIds(List<string> ids)
        {
            List<MetaData> metas = new List<MetaData>();
            foreach (string id in ids)
            {
                metas.Add(GetByID(id));
            }
            return metas;
        }

        MetaData GetByID(string id)
        {
            Console.WriteLine("Sending Request...");
            var client = new RestClient($"https://imdb8.p.rapidapi.com/title/get-meta-data?region=US&ids={id}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860");
            var response = client.Execute(request);
            var text = response.Content;

            return JSONInterpreter.ReadMetaData(text);
        }

    }
        
}
