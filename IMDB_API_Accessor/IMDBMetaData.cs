using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIAccessor
{
    public class IMDBMetaData : APIObject
    {
        public IMDBMetaData(string json)
        {
            int startIndex = 0, endIndex = 0;

            var objectName = "title";
            var titleObject = JSONInterpreter.GetObjectJSON(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "ratings";
            var ratingsObject = JSONInterpreter.GetObjectJSON(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "metacritic";
            var metacriticObject = JSONInterpreter.GetObjectJSON(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "releaseDate";
            var releaseDate = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "popularity";
            var popularity = JSONInterpreter.GetObjectJSON(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "waysToWatch";
            var waysToWatch = JSONInterpreter.GetObjectJSON(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "genres";
            var genres = JSONInterpreter.GetArrayJSON(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            var genreArray = genres.Split(",");
            for (int i = 0; i < genreArray.Length; i++)
            {
                genreArray[i] = JSONInterpreter.Clean(genreArray[i]);
            }

            objectName = "certificate";
            var certificate = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            //Get values
            //TV Show
            String imdbID = JSONInterpreter.GetValue("id", titleObject);

            String title = JSONInterpreter.GetValue("title", titleObject);

            String startYear = JSONInterpreter.GetValue("seriesStartYear", titleObject);

            String endYear = JSONInterpreter.GetValue("seriesEndYear", titleObject);

            String numEpisodes = JSONInterpreter.GetValue("numberOfEpisodes", titleObject);

            String runningTime = JSONInterpreter.GetValue("runningTimeInMinutes", titleObject);

            //Rating
            String rating = JSONInterpreter.GetValue("rating", ratingsObject);
            String ratingCount = JSONInterpreter.GetValue("ratingCount", ratingsObject);

            //WaysToWatch
            WaysToWatch ways = new WaysToWatch();
            ways.Id = JSONInterpreter.GetValue("id", waysToWatch);
            List<OptionGroup> optionGroups = new List<OptionGroup>();

            //Streaming options
            String options = JSONInterpreter.GetArrayJSON("optionGroups", waysToWatch);
            options = JSONInterpreter.RemoveArrayBrackets(options);

            while (options.Length > 0) //loop until it breaks. Exception driven development
            {
                string thisOption = JSONInterpreter.GetFirstObjectFrom(options, 0);
                options = options.Substring(thisOption.Length);

                OptionGroup group = new OptionGroup();
                group.DisplayName = JSONInterpreter.GetValue("displayName", thisOption);

                String watchOptions = JSONInterpreter.GetArrayJSON("watchOptions", thisOption);
                watchOptions = JSONInterpreter.RemoveArrayBrackets(watchOptions);
                List<WatchOption> watchOptionsList = new List<WatchOption>();

                while (watchOptions.Length > 0)
                {
                    string thisWatchOption = JSONInterpreter.GetFirstObjectFrom(watchOptions, 0);
                    watchOptions = watchOptions.Substring(thisWatchOption.Length);

                    var watchOption = new WatchOption();
                    watchOption.Primary = JSONInterpreter.GetValue("primaryText", thisWatchOption);
                    watchOption.Secondary = JSONInterpreter.GetValue("secondaryText", thisWatchOption);

                    var linkJSON = JSONInterpreter.GetObjectJSON("link", thisWatchOption);

                    watchOption.UriLink = JSONInterpreter.GetValue("uri", linkJSON);

                    watchOptionsList.Add(watchOption);
                }

                group.Options = watchOptionsList;
                optionGroups.Add(group);
            }

            ways.Groups = optionGroups;

            this.Id = imdbID;
            this.Title = title;
            this.StartYear = Int32.Parse(startYear);
            this.EndYear = Int32.Parse(endYear);
            this.NumEpisodes = Int32.Parse(numEpisodes);
            this.AverageRunningTimeMinutes = Int32.Parse(runningTime);
            this.Rating = Decimal.Parse(rating);
            this.RatingCount = Int64.Parse(ratingCount);
            this.WayToWatch = ways;
            this.Genres = genreArray;
            this.Certificate = certificate;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int NumEpisodes { get; set; }
        public int AverageRunningTimeMinutes { get; set; }
        public decimal Rating { get; set; }
        public long RatingCount { get; set; }
        public WaysToWatch WayToWatch { get; set; }
        public string[] Genres { get; set; }
        public string Certificate { get; set; }
    }

    public class WaysToWatch
    {
        public string Id { get; set; }
        public List<OptionGroup> Groups { get; set; }
    }

    public class OptionGroup
    {
        public string DisplayName { get; set; }
        public List<WatchOption> Options { get; set; }
    }

    public class WatchOption
    {
        public string Primary { get; set; }
        public string Secondary { get; set; }
        public string UriLink { get; set; }
    }
}
