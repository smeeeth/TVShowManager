using System;
using APIAccessor.API;

namespace APIAccessor.Data
{
    public class MovieDBMetaData : APIMetaData
    {
        public MovieDBMetaData(string json)
        {
            int startIndex = 0, endIndex = 0;

            var objectName = "Released";
            var releasedObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "Director";
            var directorObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "Actors";
            var actorsObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);
            
            objectName = "Plot";
            var plotObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "Language";
            var languageObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "Country";
            var countryObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "Awards";
            var awardsObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            objectName = "totalSeasons";
            var totalSeasonsObject = JSONInterpreter.GetValue(objectName, json, out startIndex, out endIndex);
            json = json.Substring(endIndex);

            Released = releasedObject;
            Director = directorObject;
            Actors = actorsObject;
            Plot = plotObject;
            Language = languageObject;
            Country = countryObject;
            Awards = awardsObject;
            TotalSeasons = Int32.Parse(totalSeasonsObject);
        }
        public string Released { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public int TotalSeasons { get; set; }

        public override string ToString()
        {
            return $"{{Released: {Released}, Director: {Director}, Actors: {Actors}, Plot: {Plot}, Language: {Language}, Country: {Country}, Awards: {Awards}, TotalSeasons: {TotalSeasons}}}";
        }
    }
}
