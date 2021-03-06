﻿using System;
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

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Not a valid id");

                using (InterpretEntities entites = new InterpretEntities())
                {
                    var join = (from m in entites.MusicFile
                                join title in entites.Title
                                on m.Title equals title.ID
                                select new { ID = m.ID, Title = new Models.TitleDTO { ID = title.ID, Name = title.NAME }, Interpret = m.Interpret, Genre = title.Genre });
                    var interprets = (join.Where(m => m.Interpret == id)).ToList();

                    // delete all music files with the given interpret id
                    interprets.ForEach(interpret =>
                    {
                        entites.MusicFile.Remove(entites.MusicFile.FirstOrDefault(m => m.ID == interpret.ID));
                        entites.SaveChanges();
                    });

                    // delete all titles for the given interpret
                    interprets.ForEach(interpret =>
                    {
                        entites.Title.Remove(entites.Title.FirstOrDefault(t => t.ID == interpret.Title.ID));
                        entites.SaveChanges();
                    });

                    // delete the interpret
                    entites.Interpret.Remove(entites.Interpret.FirstOrDefault(i => i.ID == id));
                    entites.SaveChanges();

                    return Ok("Interpret was successfully deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post()
        {
            try
            {
                using (InterpretEntities entites = new InterpretEntities())
                {
                    Models.InterpretJSON interpret = new Models.InterpretJSON(Request.Content.ReadAsStringAsync().Result);

                    // check if the interpret is already in the database 
                    var element = entites.Interpret.FirstOrDefault(i => i.NAME == interpret.Name);
                    if (element == null)
                    {
                        // create the interpret
                        InterpretDataAccess.Interpret createdInterpret = new InterpretDataAccess.Interpret();
                        createdInterpret.NAME = interpret.Name;
                        entites.Interpret.Add(createdInterpret);
                        entites.SaveChanges();

                        // return success message
                        return Ok("Interpret successfully created!");
                    }
                    else
                    {
                        return Ok("Interpret already exists");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put()
        {
            try
            {
                using (InterpretEntities entites = new InterpretEntities())
                {
                    Models.InterpretJSON interpret = new Models.InterpretJSON(Request.Content.ReadAsStringAsync().Result);

                    // check if the interpret is in the database
                    var existingInterpret = entites.Interpret.FirstOrDefault(i => i.ID == interpret.ID);
                    if (existingInterpret != null)
                    {
                        // check if another interpret in the database has the same name
                        var element = entites.Interpret.FirstOrDefault(i => i.ID != interpret.ID && i.NAME == interpret.Name);
                        if (element == null)
                        {
                            existingInterpret.NAME = interpret.Name;
                            entites.SaveChanges();
                        } else
                        {
                            return Ok("An interpret with the same name already exists.");
                        }
                    } else
                    {
                        return Ok("Cant´t edit the interpret since it doesn´t exist in the database");
                    }

                    return Ok("Interpret successfully updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
