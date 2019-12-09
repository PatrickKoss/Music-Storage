using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStorageAPI.Models
{
    public class TitleWithID
    {
        public Nullable<int> ID;
        public string Name;
        public string Genre;

        public TitleWithID(int ID, string Name, string Genre)
        {
            this.Genre = Genre;
            this.Name = Name;
            this.ID = ID;
        }
    }
}