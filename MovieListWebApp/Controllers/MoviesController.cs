using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using MovieListWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieListWebApp.Controllers
{
    public class MoviesController : Controller
    {
        private Context db = new Context();

        // GET: Movies
        public ActionResult Index(string sortOrder)
        {
            List<string> posters = new List<string>();
            List<int> counts = new List<int>();
            List<Movie> movies = db.Movies.Include(m => m.Users).ToList();


            switch (sortOrder)
            {
                case "Title":
                    movies = movies.OrderBy(m => m.Title).ToList();
                    break;
                case "Year":
                    movies = movies.OrderByDescending(m => m.ReleaseYear).ToList();
                    break;
                case "Popularity":
                    movies = movies.OrderByDescending(m => m.Users != null ? m.Users.Count : 0).ToList();
                    break;
                case "MovieID":
                    movies = movies.OrderBy(m => m.MovieId).ToList();
                    break;
                case "Random":
                    Random random = new Random();
                    movies = movies.OrderBy(m => random.Next()).ToList();
                    break;
                default:
                    break;
            }

            UserMovieViewModel viewModel = new UserMovieViewModel();
            viewModel.Movies = movies;
            ViewBag.SelectedSortMethod = sortOrder;
            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult MovieChildView(int MovieId)
        {

            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);
                Movie movie = movieRepo.GetById(MovieId);
                if (movie == null)
                {
                    return PartialView();
                }
                else
                {
                    // Task.Run = kør denne "async" funktion i baggrunden! (Bemærk: denne ActionResult er ikke "async")
                    var newPosterTask = Task.Run(() => GetMoviePosterAsync(movie.Title, movie.ReleaseYear));
                    string newPoster = newPosterTask.Result; // .Result er en del af "Task", henter resultatet ned, og blokkerer tråden indtil den task er færdig.
                    if (string.IsNullOrEmpty(newPoster))
                        {
                            newPoster = "https://i.imgur.com/uPKwCgl.jpg"; // "ingen poster fundet"
                        }
                        int count = movieRepo.GetNumberOfUsersWithMovie(movie.MovieId);


                    ViewBag.Poster = newPoster;
                    return PartialView(movie);

                }
            }
        }

        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.Id = id;
            if (id == null || id <= 0)
            {
                ViewBag.Errormsg = "No ID provided. Redirecting to Movies Index";
                return RedirectToAction("Index");
            }

            Movie movie2 = db.Movies.Include(m => m.Users).Where(u => u.MovieId == id).FirstOrDefault();
            if (movie2 == null)
            {
                ViewBag.Errormsg = "Id Provided, but no movie with such ID exists. Redirecting to Index";
                return RedirectToAction("Index");
            } else
            {
                ViewBag.PosterUrl = await GetMoviePosterAsync(movie2.Title, movie2.ReleaseYear);
                if (string.IsNullOrEmpty(ViewBag.PosterUrl))
                {
                    ViewBag.PosterUrl = "https://i.imgur.com/uPKwCgl.jpg";
                }
                
            }

            UserMovieViewModel viewModel = new UserMovieViewModel
            {
                Users = new List<User>(),
                Movies = new List<Movie> { movie2 },
            }; 
            foreach (User u in movie2.Users)
            {
                viewModel.Users.Add(u);
            }

            return View(viewModel);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieId,ReleaseYear,Title,Description")] Movie movie)
        {
            var dbMovie = db.Movies.FirstOrDefault(m => m.Title == movie.Title && m.ReleaseYear == movie.ReleaseYear);
            if (dbMovie != null)
            {
                ModelState.AddModelError("Title", "Movie already exists. Title + Year has to be unique.");
                return View(movie);
            }

            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,ReleaseYear,Title,Description")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            if(movie != null)
            {
                db.Movies.Remove(movie);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet]
        public async Task<string> GetMoviePosterAsync(string movieTitle, int year)
        {
            // API keys: 
            /*
             affe15efb5mshd7840ca7de0fdaap17c9e4jsnc9a00b91540d // ny
             265356d471mshc1d23de8b95f56cp148e18jsnfc85cfae11ce // personlig (I brug)
            API'en kan kun have ~2000 requests om måneden. Hvis den key i koden herunder ikke virker, så paste den anden ind :)
             */
            try
            {
                if (movieTitle != null && year != null)
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "265356d471mshc1d23de8b95f56cp148e18jsnfc85cfae11ce");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "moviesdatabase.p.rapidapi.com");

                    var response = await client.GetAsync($"https://moviesdatabase.p.rapidapi.com/titles/search/title/{movieTitle}?exact=true&year={year}");
                    var data = await response.Content.ReadAsStringAsync();
                    ViewBag.Data = data;
                    var json = JObject.Parse(data);

                    var results = json["results"];
                    if (results != null && results.Count() > 0)
                    {
                        var primaryImage = results[0]["primaryImage"];
                        if (primaryImage != null && primaryImage.Type != JTokenType.Null)
                        {
                            var posterUrl = primaryImage["url"].ToString();
                            return posterUrl;
                        }
                    }

                }
            } catch (Exception ex)
            {
                ViewBag.error = ex.Message;
            }

            return string.Empty;
        }

    }
}
