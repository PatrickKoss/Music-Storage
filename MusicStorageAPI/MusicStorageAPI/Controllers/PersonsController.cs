using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserDataAccess;

namespace MusicStorageAPI.Controllers
{
    public class PersonsController : ApiController
    {
        public IEnumerable<Persons> Get()
        {
            using(UserDetailsEntites entites = new UserDetailsEntites())
            {
                return entites.Persons.ToList();
            }
        }

        public Persons Get(int id)
        {
            using(UserDetailsEntites entites = new UserDetailsEntites())
            {
                return entites.Persons.FirstOrDefault(e => e.ID == id);
            }
        }
    }
}
