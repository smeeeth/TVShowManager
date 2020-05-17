using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMDB_API_Accessor
{
    public class MetaData
    {
        public string Id;
        public string Title;
        public int StartYear;
        public int EndYear;
        public int NumEpisodes;
        public int AverageRunningTimeMinutes;
        public decimal Rating;
        public long RatingCount;
        public WayToWatch WayToWatch;
        public String[] Genres;
        public String Certificate;
    }

    public class WayToWatch
    {
        public String Id;
        public List<OptionGroup> Groups;
    }

    public class OptionGroup
    {
        public String DisplayName;
        public List<WatchOption> Options;
    }

    public class WatchOption
    {
        public String Primary;
        public String Secondary;
        public String UriLink;
    }
}
