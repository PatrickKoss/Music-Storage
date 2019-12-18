using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MusicFileDataAccess;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace MusicStorageAPI.Controllers
{
    [RoutePrefix("api/musicfile")]
    public class MusicFileController : ApiController
    {
        [Route("")]
        public List<Models.MusicFileDTO> Get()
        {
            using (MusicFileEntities entites = new MusicFileEntities())
            {
                //join the tables and return the object list
                return (from musicFile in entites.MusicFile
                        join interpret in entites.Interpret
                        on musicFile.Interpret equals interpret.ID
                        join title in entites.Title
                        on musicFile.Title equals title.ID
                        select new Models.MusicFileDTO { ID = musicFile.ID, Title = new Models.TitleDTO { ID = title.ID, Name = title.NAME }, Interpret = new Models.InterpretDTO { ID = interpret.ID, Name = interpret.NAME }, Genre = title.Genre }).ToList();
            }
        }

        [Route("{id}")]
        public Models.MusicFileDTO Get(int id)
        {
            using (MusicFileEntities entites = new MusicFileEntities())
            {
                // join the list and return a specific music file by id
                var join = (from musicFile in entites.MusicFile
                            join interpret in entites.Interpret
                            on musicFile.Interpret equals interpret.ID
                            join title in entites.Title
                            on musicFile.Title equals title.ID
                            select new { ID = musicFile.ID, Title = new Models.TitleDTO { ID = title.ID, Name = title.NAME }, Interpret = new Models.InterpretDTO { ID = interpret.ID, Name = interpret.NAME }, Genre = title.Genre });
                return join.Select(
                m => new Models.MusicFileDTO { ID = m.ID, Title = new Models.TitleDTO { ID = m.Title.ID, Name = m.Title.Name }, Interpret = new Models.InterpretDTO { ID = m.Interpret.ID, Name = m.Interpret.Name }, Genre = m.Genre }).SingleOrDefault(m => m.ID == id);
            }
        }

        [Route("sortBy={sortBy?}/search={search?}/filter/interpret={interpretName}/genre={genreName}")]
        public List<Models.MusicFileDTO> Get(string sortBy, string search, string interpretName, string genreName)
        {
            using (MusicFileEntities entites = new MusicFileEntities())
            {
                //join the tables
                var join = (from musicFile in entites.MusicFile
                            join interpret in entites.Interpret
                            on musicFile.Interpret equals interpret.ID
                            join title in entites.Title
                            on musicFile.Title equals title.ID
                            select new { ID = musicFile.ID, Title = new Models.TitleDTO { ID = title.ID, Name = title.NAME }, Interpret = new Models.InterpretDTO { ID = interpret.ID, Name = interpret.NAME }, Genre = title.Genre });
                if (search != "null")
                {
                    // filter the results by the search
                    var searchedInterprets = join.Where(i => i.Title.Name.Contains(search) || i.Interpret.Name.Contains(search) || i.Genre.Contains(search));
                    // filter the results by the given interpret
                    if (interpretName != "null")
                    {
                        searchedInterprets = searchedInterprets.Where(i => i.Interpret.Name == interpretName);
                    }
                    // filter the results by the given genre
                    if (genreName != "null")
                    {
                        searchedInterprets = searchedInterprets.Where(i => i.Genre == genreName);
                    }
                    // order the results
                    var orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                    if (!(sortBy == "null"))
                    {
                        if (sortBy == "interpret")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.Interpret.Name);
                        }
                        if (sortBy == "-interpret")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.Interpret.Name);
                        }
                        if (sortBy == "id")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                        }
                        if (sortBy == "-id")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.ID);
                        }
                        if (sortBy == "title")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.Title.Name);
                        }
                        if (sortBy == "-title")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.Title.Name);
                        }
                        if (sortBy == "genre")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.Genre);
                        }
                        if (sortBy == "-genre")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.Genre);
                        }
                    }
                    return (from musicFile in orderedInterprets
                            select new Models.MusicFileDTO { ID = musicFile.ID, Title = new Models.TitleDTO { ID = musicFile.Title.ID, Name = musicFile.Title.Name }, Interpret = new Models.InterpretDTO { ID = musicFile.Interpret.ID, Name = musicFile.Interpret.Name }, Genre = musicFile.Genre }).ToList();
                }
                else
                {
                    // do the same as above
                    var searchedInterprets = join;
                    if (interpretName != "null")
                    {
                        searchedInterprets = searchedInterprets.Where(i => i.Interpret.Name == interpretName);
                    }
                    if (genreName != "null")
                    {
                        searchedInterprets = searchedInterprets.Where(i => i.Genre == genreName);
                    }
                    var orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                    if (!(sortBy == "null"))
                    {
                        if (sortBy == "interpret")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.Interpret.Name);
                        }
                        if (sortBy == "-interpret")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.Interpret.Name);
                        }
                        if (sortBy == "id")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.ID);
                        }
                        if (sortBy == "-id")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.ID);
                        }
                        if (sortBy == "title")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.Title.Name);
                        }
                        if (sortBy == "-title")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.Title.Name);
                        }
                        if (sortBy == "genre")
                        {
                            orderedInterprets = searchedInterprets.OrderBy(i => i.Genre);
                        }
                        if (sortBy == "-genre")
                        {
                            orderedInterprets = searchedInterprets.OrderByDescending(i => i.Genre);
                        }
                    }
                    return (from musicFile in orderedInterprets
                            select new Models.MusicFileDTO { ID = musicFile.ID, Title = new Models.TitleDTO { ID = musicFile.Title.ID, Name = musicFile.Title.Name }, Interpret = new Models.InterpretDTO { ID = musicFile.Interpret.ID, Name = musicFile.Interpret.Name }, Genre = musicFile.Genre }).ToList();
                }
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post()
        {
            try
            {
                using (MusicFileEntities entites = new MusicFileEntities())
                {
                    Models.MusicFileJson musicFile = new Models.MusicFileJson(Request.Content.ReadAsStringAsync().Result);
                    // check if the genre is valid
                    if (!(musicFile.Genre == "Pop" || musicFile.Genre == "Rock" || musicFile.Genre == "Dance" || musicFile.Genre == "Latin"))
                    {
                        return Ok("Not a correct genre");
                    }
                    //System.Diagnostics.Debug.WriteLine(musicFile.Title.Name);
                    var join = (from m in entites.MusicFile
                                join title in entites.Title
                                on m.Title equals title.ID
                                select new { ID = m.ID, Title = new Models.TitleDTO { ID = title.ID, Name = title.NAME }, Interpret = m.Interpret, Genre = title.Genre });

                    // check if the title is already in the database 
                    var element = join.FirstOrDefault(m => m.Title.Name == musicFile.Title.Name && m.Interpret == musicFile.Interpret.ID);
                    if (element == null)
                    {
                        //First Create the title
                        MusicFileDataAccess.Title title = new MusicFileDataAccess.Title();
                        title.NAME = musicFile.Title.Name;
                        title.Genre = musicFile.Genre;
                        entites.Title.Add(title);
                        entites.SaveChanges();
                        // get the Title ID
                        Models.TitleDTO createdTitle = entites.Title.Select(i => new Models.TitleDTO()
                        {
                            ID = i.ID,
                            Name = i.NAME
                        }).OrderByDescending(t => t.ID).FirstOrDefault(i => i.Name == musicFile.Title.Name);
                        System.Diagnostics.Debug.WriteLine(createdTitle.ID);

                        // if the interpret is not in the database, create a new interpret
                        var existingInterpret = entites.Interpret.FirstOrDefault(i => i.NAME == musicFile.Interpret.Name);
                        Models.InterpretDTO createdInterpret = new Models.InterpretDTO { ID = musicFile.Interpret.ID, Name = musicFile.Interpret.Name };
                        if (existingInterpret == null)
                        {
                            MusicFileDataAccess.Interpret interpret = new MusicFileDataAccess.Interpret();
                            interpret.NAME = musicFile.Interpret.Name;
                            entites.Interpret.Add(interpret);
                            entites.SaveChanges();
                            // after the creation get the newly id of the interpret
                            createdInterpret = entites.Interpret.Select(i => new Models.InterpretDTO()
                            {
                                ID = i.ID,
                                Name = i.NAME
                            }).SingleOrDefault(i => i.Name == musicFile.Interpret.Name);
                        }
                        System.Diagnostics.Debug.WriteLine(createdTitle.ID);
                        //in the last step create the music file
                        MusicFileDataAccess.MusicFile createdMusicFile = new MusicFileDataAccess.MusicFile();
                        createdMusicFile.Title = createdTitle.ID;
                        createdMusicFile.Interpret = createdInterpret.ID;
                        entites.MusicFile.Add(createdMusicFile);
                        entites.SaveChanges();

                        // return success message
                        return Ok("Music file successfully created!");
                    } else
                    {
                        return Ok("Element is already in the database");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
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

                using (MusicFileEntities entites = new MusicFileEntities())
                {
                    //get title id to delete the title after deleting the music file
                    var musicFile = entites.MusicFile.FirstOrDefault(m => m.ID == id);
                    int titleID = musicFile.Title;

                    // delete the music file
                    entites.MusicFile.Remove(entites.MusicFile.FirstOrDefault(m => m.ID == id));
                    entites.SaveChanges();

                    //delete the title
                    entites.Title.Remove(entites.Title.FirstOrDefault(t => t.ID == titleID));
                    entites.SaveChanges();

                    return Ok("Music file successfully deleted!");
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
                using (MusicFileEntities entites = new MusicFileEntities())
                {
                    Models.MusicFileJson musicFile = new Models.MusicFileJson(Request.Content.ReadAsStringAsync().Result);
                    // check if the genre is valid
                    if (!(musicFile.Genre == "Pop" || musicFile.Genre == "Rock" || musicFile.Genre == "Dance" || musicFile.Genre == "Latin"))
                    {
                        return Ok("Not a correct genre");
                    }
                    var join = (from m in entites.MusicFile
                                join title in entites.Title
                                on m.Title equals title.ID
                                select new { ID = m.ID, Title = new Models.TitleDTO { ID = title.ID, Name = title.NAME }, Interpret = m.Interpret, Genre = title.Genre });

                    // check if the interpret already exists in the database
                    var preInterpret = entites.Interpret.FirstOrDefault(i => i.NAME == musicFile.Interpret.Name);
                    Models.InterpretDTO createdInterpret = new Models.InterpretDTO { ID = 0, Name = "" };
                    if (preInterpret != null)
                    {
                        createdInterpret.ID = preInterpret.ID;
                        createdInterpret.Name = preInterpret.NAME;
                        // check if the interpret already has a title named like the musicfile
                        var tempTitle = join.FirstOrDefault(m => m.Interpret == createdInterpret.ID && m.Title.Name == musicFile.Title.Name && m.Title.ID != musicFile.Title.ID);
                        if (tempTitle != null)
                        {
                            return Ok("The interpret " + createdInterpret.Name + " has already a title named like " + musicFile.Title.Name);
                        }
                    } else
                    {
                        //create new interpret
                        MusicFileDataAccess.Interpret interpret = new MusicFileDataAccess.Interpret();
                        interpret.NAME = musicFile.Interpret.Name;
                        entites.Interpret.Add(interpret);
                        entites.SaveChanges();
                        // after the creation get the newly id of the interpret
                        createdInterpret = entites.Interpret.Select(i => new Models.InterpretDTO()
                        {
                            ID = i.ID,
                            Name = i.NAME
                        }).SingleOrDefault(i => i.Name == musicFile.Interpret.Name);
                    }
                    var existingTitle = entites.Title.FirstOrDefault(t => t.ID == musicFile.Title.ID);
                    if (existingTitle != null)
                    {
                        existingTitle.NAME = musicFile.Title.Name;
                        existingTitle.Genre = musicFile.Genre;
                        entites.SaveChanges();

                        var existingMusicFile = entites.MusicFile.FirstOrDefault(m => m.ID == musicFile.ID);
                        
                        if (existingMusicFile != null)
                        {
                            existingMusicFile.Interpret = createdInterpret.ID;
                            entites.SaveChanges();
                        } else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                    return Ok("Successfully updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
