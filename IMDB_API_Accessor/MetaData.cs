using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMDB_API_Accessor
{
    public class MetaData
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int NumEpisodes { get; set; }
        public int AverageRunningTimeMinutes { get; set; }
        public decimal Rating { get; set; }
        public long RatingCount { get; set; }
        public WayToWatch WayToWatch { get; set; }
        public String[] Genres { get; set; }
        public String Certificate { get; set; }
    }

    public class WayToWatch
    {
        public String Id { get; set; }
        public List<OptionGroup> Groups { get; set; }
    }

    public class OptionGroup
    {
        public String DisplayName { get; set; }
        public List<WatchOption> Options { get; set; }
    }

    public class WatchOption
    {
        public String Primary { get; set; }
        public String Secondary { get; set; }
        public String UriLink { get; set; }
    }
}
