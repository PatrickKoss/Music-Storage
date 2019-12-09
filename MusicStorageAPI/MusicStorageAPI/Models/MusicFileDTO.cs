using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStorageAPI.Models
{
    public class MusicFileDTO
    {
        public int ID { get; set; }
        public Models.TitleDTO Title { get; set; }
        public Models.InterpretDTO Interpret { get; set; }
        public string Genre { get; set; }
    }
}