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

            MetaData meta = ReadMetaData(text);

            Console.WriteLine(meta);

            Console.ReadLine();

            MetaDataProcessor processor = new MetaDataProcessor();
            processor.ProcessMetaDatas(new List<MetaData> { meta });

            for (int i = 0; i<2; i++)
            {
                for (int j=0; j<14; j++)
                {
                    Console.Write(processor.MainTable[i,j] + "\t");
                }
                Console.WriteLine();
            }


            Console.ReadLine();
        }

        static MetaData GetByID(string id)
        {
            Console.WriteLine("Sending Request...");
            var client = new RestClient($"https://imdb8.p.rapidapi.com/title/get-meta-data?region=US&ids={id}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "53ea5ed115mshbf90e8763756573p196d74jsn92f297750860");
            var response = client.Execute(request);
            var text = response.Content;

            return ReadMetaData(text);
        }

        static MetaData ReadMetaData(String text)
        {
            int startIndex = 0, endIndex = 0;

            var objectName = "title";
            var titleObject = GetObjectJSON(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            objectName = "ratings";
            var ratingsObject = GetObjectJSON(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            objectName = "metacritic";
            var metacriticObject = GetObjectJSON(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            objectName = "releaseDate";
            var releaseDate = GetValue(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            objectName = "popularity";
            var popularity = GetObjectJSON(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            objectName = "waysToWatch";
            var waysToWatch = GetObjectJSON(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            objectName = "genres";
            var genres = GetArrayJSON(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            var genreArray = genres.Split(",");
            for (int i=0; i< genreArray.Length; i++)
            {
                genreArray[i] = Clean(genreArray[i]);
            }
    
            objectName = "certificate";
            var certificate = GetValue(objectName, text, out startIndex, out endIndex);
            text = text.Substring(endIndex);

            //Get values
            //TV Show
            String imdbID = GetValue("id", titleObject);

            String title = GetValue("title", titleObject);

            String startYear = GetValue("seriesStartYear", titleObject);

            String endYear = GetValue("seriesEndYear", titleObject);

            String numEpisodes = GetValue("numberOfEpisodes", titleObject);

            String runningTime = GetValue("runningTimeInMinutes", titleObject);

            //Rating
            String rating = GetValue("rating", ratingsObject);
            String ratingCount = GetValue("ratingCount", ratingsObject);

            //WaysToWatch
            WayToWatch ways = new WayToWatch();
            ways.Id = GetValue("id", waysToWatch);
            List<OptionGroup> optionGroups = new List<OptionGroup>();

            //Streaming options
            String options = GetArrayJSON("optionGroups", waysToWatch);
            options = RemoveArrayBrackets(options);

            while (options.Length > 0) //loop until it breaks. Exception driven development
            {
                string thisOption = GetFirstObjectFrom(options, 0);
                options = options.Substring(thisOption.Length);

                OptionGroup group = new OptionGroup();
                group.DisplayName = GetValue("displayName", thisOption);
               
                String watchOptions = GetArrayJSON("watchOptions", thisOption);
                watchOptions = RemoveArrayBrackets(watchOptions);
                List<WatchOption> watchOptionsList = new List<WatchOption>();

                while (watchOptions.Length>0)
                {                  
                    string thisWatchOption = GetFirstObjectFrom(watchOptions, 0);
                    watchOptions = watchOptions.Substring(thisWatchOption.Length);

                    var watchOption = new WatchOption();
                    watchOption.Primary = GetValue("primaryText", thisWatchOption);
                    watchOption.Secondary = GetValue("secondaryText", thisWatchOption);

                    var linkJSON = GetObjectJSON("link", thisWatchOption);

                    watchOption.UriLink = GetValue("uri", linkJSON);

                    watchOptionsList.Add(watchOption);
                }

                group.Options = watchOptionsList;
                optionGroups.Add(group);
            }

            ways.Groups = optionGroups;

            MetaData metadata = new MetaData()
            {
                Id = imdbID,
                Title = title,
                StartYear = Int32.Parse(startYear),
                EndYear = Int32.Parse(endYear),
                NumEpisodes = Int32.Parse(numEpisodes),
                AverageRunningTimeMinutes = Int32.Parse(runningTime),
                Rating = Decimal.Parse(rating),
                RatingCount = Int64.Parse(ratingCount),
                WayToWatch = ways,
                Genres = genreArray,
                Certificate = certificate
            };

            return metadata;

        }

        static string GetArrayJSON(String name, String JSON)
        {
            return GetObjectJSON(name, JSON, '[', ']');
        }

        static string GetArrayJSON(String name, String JSON, out int startIndex, out int endIndex)
        {
            return GetObjectJSON(name, JSON, out startIndex, out endIndex, '[', ']');
        }

        static string GetObjectJSON(String name, String JSON, char open = '{', char close = '}')
        {
            return GetObjectJSON(name, JSON, out int start, out int end, open, close);
        }

        //returns first object with specified name
        static string GetObjectJSON(String name, String JSON, out int startIndex, out int endIndex, char open = '{', char close = '}')
        {
            startIndex = JSON.IndexOf(name);

            int index = JSON.IndexOf(open, startIndex);

            var thisObject = GetFirstObjectFrom(JSON, index, open, close);

            endIndex = JSON.IndexOf(thisObject) + thisObject.Length;

            return thisObject;
        }

        static string GetFirstObjectFrom(String JSON, int startIndex, char open = '{', char close = '}')
        {
            string thisObject = "";

            List<char> brackets = new List<char>();

            Char[] JSONArray = JSON.ToCharArray();

            bool foundBracket = false;

            for (int i = startIndex; i < JSONArray.Length && (foundBracket == false || brackets.Count > 0); i++)
            {
                thisObject += JSONArray[i];
                if (JSONArray[i] == open)
                {
                    foundBracket = true;
                    brackets.Add(JSONArray[i]);
                }
                else if (JSONArray[i] == close)
                {
                    brackets.RemoveAt(0);
                }
            }
            return thisObject;
        }

        static String GetValue(String name, String JSON)
        {
            return GetValue(name, JSON, out int start, out int end);
        }

        static String GetValue(String name, String JSON, out int startIndex, out int endIndex)
        {
            name = $"\"{name}\"";
            startIndex = JSON.IndexOf(name);

            int index = JSON.IndexOf(":", startIndex) + 1;

            string thisValue = "";

            Char[] JSONArray = JSON.ToCharArray();

            for (int i = index; i < JSONArray.Length && JSONArray[i] != ','; i++)
            {
                thisValue += JSONArray[i];
            }

            endIndex = JSON.IndexOf(thisValue) + thisValue.Length;

            return Clean(thisValue);

        }

        static string Clean(string value)
        {
            string startChars = "\"{[ ";
            var valueArray = value.ToCharArray();
            var startCharsArray = startChars.ToCharArray();
            for (int i = 0; i < value.Length; i++)
            {
                if (startChars.Contains(value[i])) {
                    value = value.Remove(i--, 1);
                }
                else
                {
                    break;
                }
            }


            string endChars = "}] \"";
            for (int i = value.Length - 1; i > 0; i--) {
                if (endChars.Contains(value[i]))
                {
                    value = value.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }

            return value;
        }

        static string RemoveArrayBrackets(string JSONArray)
        {
            string startChars = "[";
            for (int i = 0; i < JSONArray.Length; i++)
            {
                if (startChars.Contains(JSONArray[i]))
                {
                    JSONArray = JSONArray.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }


            string endChars = "]";
            for (int i = JSONArray.Length - 1; i > 0; i--)
            {
                if (endChars.Contains(JSONArray[i]))
                {
                    JSONArray = JSONArray.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }

            return JSONArray;
        }


    }
}
