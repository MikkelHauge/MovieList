using BusinessLogicLayer.Model;
using DataAccessLayer.Context;
using DataAccessLayer.DTO;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MovieListAPI.Controllers
{
    public class MoviesAPIController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Henter samtlige film inklusiv id på users, som er knyttet til filmen.
        /// </summary>
        /// <returns>List of Movies</returns>
        [HttpGet]
        public IEnumerable<MovieDTO> GetAll()
        {
            using (var context = new Context())
            {
                var movieRepo = new MovieRepository(context);
                var movies = movieRepo.GetAll();
                var movieDTOs = new List<MovieDTO>();
                foreach (var movie in movies)
                {
                    var movieDTO = new MovieDTO
                    {
                        MovieId = movie.MovieId,
                        ReleaseYear = movie.ReleaseYear,
                        Title = movie.Title,
                        Description = movie.Description,
                        UserIds = movie.Users.Select(u => u.UserId).ToList()

                    };
                    movieDTOs.Add(movieDTO);
                }
                return movieDTOs;
            }
        }

        // GET api/values/5
        /// <summary>
        /// Returnerer en movie med et bestemt ID
        /// </summary>
        /// <returns>Single Movie</returns>
        public MovieDTO Get(int id)
        {
            using (var context = new Context())
            {
                var movieRepo = new MovieRepository(context);
                var movie = movieRepo.GetById(id);
                var movieDTO = new MovieDTO
                {
                    MovieId = movie.MovieId,
                    ReleaseYear = movie.ReleaseYear,
                    Title = movie.Title,
                    Description = movie.Description,
                    UserIds = movie.Users.Select(u => u.UserId).ToList()
                };
                return movieDTO;
            }
        }

        // POST api/values
        /// <summary>
        /// Tilføjer en ny film til databasen. Filmen må ikke eksistere i forvejen.
        /// </summary>
        /// <returns>String</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody] MovieDTO NewMovieDTO)
        {
            using (var context = new Context())
            {
                var movieRepo = new MovieRepository(context);
                Movie movie = new Movie();
                movie.Title = NewMovieDTO.Title;
                movie.ReleaseYear = NewMovieDTO.ReleaseYear;
                movie.Description = NewMovieDTO.Description;
                try
                {
                    movieRepo.Add(movie);
                    context.SaveChanges();
                } catch(Exception ex)
                {
                    return BadRequest("Movie not created. It might already exists. Full Error Message: " + ex.Message);

                }
            }
            return Ok("The movie has successfully been added to the database.");
        }

        // PUT api/values/5
        /// <summary>
        /// Opdaterer en film, fra databasen.
        /// </summary>
        /// <returns>Nothing?</returns>
        public IHttpActionResult Put(int id, [FromBody] MovieDTO movieToUpdate)
        {
            using (var context = new Context())
            {
                var movieRepo = new MovieRepository(context);
                Movie movie = movieRepo.GetById(id);
                movie.Title = movieToUpdate.Title;
                movie.ReleaseYear = movieToUpdate.ReleaseYear;
                movie.Description = movieToUpdate.Description;
                try
                {
                    movieRepo.Update(movie);
                    context.SaveChanges();
                    return Ok("The movie has successfully been updated.");
                }
                catch (Exception ex)
                {
                    return BadRequest("Movie not updated. A movie with similar title + release year might already exist. Full Error Message: " + ex.Message);

                }
            }
        }

        // DELETE api/values/5
        // GET api/values/5
        /// <summary>
        /// Sletter en film i databasen.
        /// </summary>
        /// <returns>Nothing?</returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (var context = new Context())
            {
                var movieRepo = new MovieRepository(context);
                Movie movieToBeDeleted = movieRepo.GetById(id);
                if (movieToBeDeleted == null) {

                    return BadRequest("Delete failed. No movie with such an ID was found.");
                }
                try
                {
                    movieRepo.Delete(movieToBeDeleted);
                    context.SaveChanges();
                    
                }
                catch (Exception ex)
                {
                    return BadRequest("Movie not deleted. Full Error Message: " + ex.Message);

                }
                return Ok("The movie has successfully been deleted.");
            }
        }
    }
}
