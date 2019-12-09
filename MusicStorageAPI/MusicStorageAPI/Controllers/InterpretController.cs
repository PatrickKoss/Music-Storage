using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InterpretDataAccess;
using System.Reflection;

namespace MusicStorageAPI.Controllers
{
    [RoutePrefix("api/interpret")]
    public class InterpretController : ApiController
    {
        [Route("")]
        public List<Models.InterpretDTO> Get()
        {
            using (InterpretEntities entites = new InterpretEntities())
            {
                return (from interpret in entites.Interpret
                        select new Models.InterpretDTO { ID = interpret.ID, Name = interpret.NAME}).ToList();
            }
        }

        [Route("sortBy={sortBy?}/search={search?}")]
        public List<Models.InterpretDTO> Get(string sortBy = null, string search = null)
        {
            using (InterpretEntities entites = new InterpretEntities())
            {
                if (search != "null")
                {
                    var searchedInterprets = entites.Interpret.Where(i => i.NAME.Contains(search));
                    var orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                    if (!(sortBy == "null"))
                    {
                        if (sortBy == "name")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.NAME);
                        }
                        if (sortBy == "-name")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.NAME);
                        }
                        if (sortBy == "id")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                        }
                        if (sortBy == "-id")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.ID);
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("hello1");
                    System.Diagnostics.Debug.WriteLine(orderedInterprets);
                    return (from interpret in orderedInterprets
                     select new Models.InterpretDTO { Name = interpret.NAME, ID = interpret.ID }).ToList();
                }
                else
                {
                    var searchedInterprets = entites.Interpret;
                    var orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                    if (!(sortBy == "null"))
                    {
                        if (sortBy == "name")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.NAME);
                        }
                        if (sortBy == "-name")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.NAME);
                        }
                        if (sortBy == "id")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                        }
                        if (sortBy == "-id")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.ID);
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(orderedInterprets);
                    return (from interpret in orderedInterprets
                     select new Models.InterpretDTO { Name = interpret.NAME, ID = interpret.ID }).ToList();
                }
            }
        }

        [Route("{id}")]
        public Models.InterpretDTO Get(int id)
        {
            using (InterpretEntities entites = new InterpretEntities())
            {
                return entites.Interpret.Select(i => new Models.InterpretDTO()
                {
                    ID = i.ID,
                    Name = i.NAME
                }).SingleOrDefault(i => i.ID == id);
            }
        }
    }
}
