using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MusicStorageAPI.Models
{
    public class MusicFileJson
    {
        public MusicFileJson(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jMusicFile = jObject["musicFile"];
            ID = (int)jMusicFile["ID"];
            Genre = (string)jMusicFile["Genre"];
            JToken jTitle = jMusicFile["Title"];
            JToken jInterpret = jMusicFile["Interpret"];
            Title = new Models.TitleDTO { ID = (int)jTitle["ID"], Name = (string)jTitle["Name"] };
            Interpret = new Models.InterpretDTO { ID = (int)jInterpret["ID"], Name = (string)jInterpret["Name"] };
        }

        public int ID { get; set; }
        public Models.TitleDTO Title { get; set; }
        public Models.InterpretDTO Interpret { get; set; }
        public string Genre { get; set; }
    }
}