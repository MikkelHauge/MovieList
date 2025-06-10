using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using MovieListWebApp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MovieListWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<User> users;
            List<Movie> movies;

            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);
                movies = movieRepo.GetAll().Include(m => m.Users).ToList();
                users = userRepo.GetAll().Include(u => u.Movies).ToList();
            }
            UserMovieViewModel viewModel = new UserMovieViewModel
            {
                Users = users,
                Movies = movies
            };
            return View(viewModel);
        }

        public ActionResult AddMovie(int userId, int movieId)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);

                var user = userRepo.GetById(userId);
                var movie = movieRepo.GetById(movieId);

                user.Movies.Add(movie);
                context.SaveChanges();
            }
            return RedirectToAction("Details", "Users", new { id = userId });
        }

        [HttpPost]
        public ActionResult RemoveMovie(int userId, int movieId)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);

                var user = userRepo.GetById(userId);
                var movie = movieRepo.GetById(movieId);

                user.Movies.Remove(movie);
                context.SaveChanges();

                return RedirectToAction("Details", "Users", new { id = userId });
            }
        }

        public ActionResult Search(string searchword, string SearchUser, string SearchMovie)
        {
            List<User> userSearchResult = null;
            List<Movie> movieSearchResults = null;
            UserMovieViewModel viewModel = new UserMovieViewModel();

            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);

                if (SearchUser != null)
                {
                    userSearchResult = userRepo.GetAllUsersContainingName(searchword).ToList();
                }
                if (SearchMovie != null)
                {
                    movieSearchResults = movieRepo.GetAllMoviesContainingName(searchword).ToList();

                }
                viewModel.Users = userSearchResult;
                viewModel.Movies = movieSearchResults;
            }

            return View(viewModel);
        }


        public ActionResult RedirectToIndex()
        {
            // kaldes hvis du går ind på /home/search (uden at søge på noget)
            return RedirectToAction("Index");
        }

    }


}
