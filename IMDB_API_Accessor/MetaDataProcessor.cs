using System;
using System.Collections.Generic;
using System.Text;

namespace IMDB_API_Accessor
{
    class MetaDataProcessor
    {
        Dictionary<string, int> GenreColumnMap = new Dictionary<string, int>();
        int GenreColumnCount = 0;

        Dictionary<string, int> WayToWatchColumnMap = new Dictionary<string, int>();
        int WayToWatchColumnCount = 0;

        public String[,] MainTable;

        public MetaDataProcessor()
        {

        }
        public void ProcessMetaDatas(List<MetaData> metadatas)
        {
            //process genres
            foreach (MetaData meta in metadatas)
            {
                foreach (string genre in meta.Genres)
                {
                    if (!GenreColumnMap.ContainsKey(genre)) //if not already in map
                    {
                        GenreColumnMap.Add(genre, GenreColumnCount++);
                    }
                }
            }

            //process ways to watch
            foreach (MetaData meta in metadatas)
            {
                WayToWatch watch = meta.WayToWatch;

                foreach(OptionGroup group in watch.Groups)
                {
                    foreach(WatchOption option in group.Options)
                    {
                        string optionStr = $"{group.DisplayName} {option.Primary}";

                        if (!WayToWatchColumnMap.ContainsKey(optionStr)) //if not already in map
                        {
                            WayToWatchColumnMap.Add(optionStr, WayToWatchColumnCount++);
                        }
                    }
                }
            }

            // COLUMNS:
            //Id
            //Title
            //StartYear
            //EndYear
            //NumEpisodes
            //RunningTime
            //Rating
            //Rating Count
            //Certificate
            //Genre columns
            //Ways to watch columns

            MainTable = new String[metadatas.Count + 1, 9 + GenreColumnCount + WayToWatchColumnCount];

            //generate headings
            int count = 0;
            MainTable[0, count++] = "Id";
            MainTable[0, count++] = "Title";
            MainTable[0, count++] = "StartYear";
            MainTable[0, count++] = "EndYear";
            MainTable[0, count++] = "NumEpisodes";
            MainTable[0, count++] = "RunningTime";
            MainTable[0, count++] = "Rating";
            MainTable[0, count++] = "RatingCount";
            MainTable[0, count++] = "Certificate";
            foreach (String genre in GenreColumnMap.Keys)
            {
                MainTable[0, count++] = genre;
            }
            foreach (String wayToWatch in WayToWatchColumnMap.Keys)
            {
                MainTable[0, count++] = wayToWatch;
            }

            //add data
            int rowNum = 1;
            foreach(MetaData meta in metadatas)
            {
                count = 0;

                MainTable[rowNum, count++] = meta.Id;
                MainTable[rowNum, count++] = meta.Title;
                MainTable[rowNum, count++] = meta.StartYear.ToString();
                MainTable[rowNum, count++] = meta.EndYear.ToString();
                MainTable[rowNum, count++] = meta.NumEpisodes.ToString();
                MainTable[rowNum, count++] = meta.AverageRunningTimeMinutes.ToString();
                MainTable[rowNum, count++] = meta.Rating.ToString();
                MainTable[rowNum, count++] = meta.RatingCount.ToString();
                MainTable[rowNum, count++] = meta.Certificate;
                for (int i = 0; i < meta.Genres.Length; i++)
                {
                    int colInd = -1;
                    if (GenreColumnMap.TryGetValue(meta.Genres[i], out colInd))
                    {
                        MainTable[rowNum, count + colInd] = (i+1).ToString();
                    }
                }
                count += GenreColumnCount;
                foreach(OptionGroup group in meta.WayToWatch.Groups)
                {
                    foreach(WatchOption watchOption in group.Options)
                    {
                        int colInd = -1;
                        if (WayToWatchColumnMap.TryGetValue($"{group.DisplayName} {watchOption.Primary}", out colInd))
                        {
                            MainTable[rowNum, count + colInd] = watchOption.Secondary;
                        }
                    }
                }
                count += WayToWatchColumnCount;

                rowNum++;
            }


        }
    }
}
