using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.DTO;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MovieListAPI.Controllers
{
    public class UsersAPIController : ApiController
    {
        /// <summary>
        /// Henter samtlige Users inklusiv id på filmene, som er knyttet til de enkelte users.
        /// </summary>
        /// <returns>All users</returns>
        [HttpGet]
        public IEnumerable<UserDTO> GetAll()
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var users = userRepo.GetAll();
                var userDTOs = new List<UserDTO>();
                foreach (var user in users)
                {
                    var userDTO = new UserDTO
                    {
                        UserId = user.UserId,
                        Name = user.Name,
                        MovieIds = user.Movies.Select(u => u.MovieId).ToList()
                    };
                    userDTOs.Add(userDTO);
                }
                return userDTOs;
            }
        }
        /// <summary>
        /// Returnerer en User med et bestemt ID.
        /// </summary>
        /// <returns>Specific User based on ID</returns>
        [HttpGet]
        public UserDTO Get(int id)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var user = userRepo.GetById(id);
                var userDTO = new UserDTO
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    MovieIds = user.Movies.Select(m => m.MovieId).ToList()
                };
                return userDTO;
            }

        }

        // POST api/values
        /// <summary>
        /// Tilføjer en ny user til databasen. Navnet må ikke eksistere i forvejen.
        /// </summary>
        /// <returns>String</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody] UserDTO NewUserDTO)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                User user = new User();
                user.Name = NewUserDTO.Name;
                try
                {
                    userRepo.Add(user);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("User not created. It might already exists. Full Error Message: " + ex.Message);

                }
            }
            return Ok("The user has successfully been added to the database.");
        }

        // PUT api/values/5
        /// <summary>
        /// Opdaterer en user, fra databasen.
        /// </summary>
        /// <returns>Result from request</returns>
        public IHttpActionResult Put(int id, [FromBody] UserDTO userToUpdate)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                User user = userRepo.GetById(id);
                user.Name = userToUpdate.Name;
                try
                {
                    userRepo.Update(user);
                    context.SaveChanges();
                    return Ok("The user has successfully been updated.");
                }
                catch (Exception ex)
                {
                    return BadRequest("User not updated. A user with similar name might already exist. Full Error Message: " + ex.Message);

                }
            }
        }

        // DELETE api/values/5
        /// <summary>
        /// Sletter en user i databasen.
        /// </summary>
        /// <returns>Result from request</returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                User userToBeDeleted = userRepo.GetById(id);
                if (userToBeDeleted == null)
                {

                    return BadRequest("Delete failed. No user with such an ID was found.");
                }
                try
                {
                    userRepo.Delete(userToBeDeleted);
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    return BadRequest("User not deleted. Full Error Message: " + ex.Message);

                }
                return Ok("The user has successfully been deleted.");
            }
        }

        // PUT 
        /// <summary>
        /// Tilføjer en movie, til en user's liste af movies.
        /// </summary>
        /// <returns>Result from request</returns>
        [HttpPut]
        [Route("api/UsersAPI/{userId}/AddMovie/{movieId}")]
        public IHttpActionResult AddMovieToUser(int userId, int movieId)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);

                var user = userRepo.GetById(userId);
                var movie = movieRepo.GetById(movieId);

                if (user == null)
                {
                    return NotFound();
                }
                if (movie == null)
                {
                    return NotFound();
                }
                try
                {
                    user.Movies.Add(movie);
                    movie.Users.Add(user);
                    context.SaveChanges();
                } catch(Exception ex)
                {
                    return BadRequest("Error. Movie is maybe already added? Full Error message: " + ex.Message);
                }
                return Ok("Movie successfully added to the User's list.");
            }
        }

        // PUT 
        /// <summary>
        /// Fjerner en movie, til en user's liste af movies.
        /// </summary>
        /// <returns>Result from request</returns>
        [HttpPut]
        [Route("api/UsersAPI/{userId}/RemoveMovie/{movieId}")]
        public IHttpActionResult RemoveMovieFromUser(int userId, int movieId)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);

                var user = userRepo.GetById(userId);
                var movie = movieRepo.GetById(movieId);

                if (user == null)
                {
                    return NotFound();
                }
                if (movie == null)
                {
                    return NotFound();
                }
                try
                {
                    user.Movies.Remove(movie);
                    movie.Users.Remove(user);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("Error. The movie might not be on the list? Full Error message: " + ex.Message);
                }
                return Ok("Movie successfully removed to the User's list.");
            }
        }
    }
}
