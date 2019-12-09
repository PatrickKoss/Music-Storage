using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStorageAPI.Models
{
    public class MusicFileWithoutIDsDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Interpret { get; set; }
        public string Genre { get; set; }
    }
}