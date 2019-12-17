using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MusicStorageAPI.Models
{
    public class InterpretJSON
    {
        public InterpretJSON(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jMusicFile = jObject["interpret"];
            ID = (int)jMusicFile["ID"];
            Name = (string)jMusicFile["Name"];
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }
}