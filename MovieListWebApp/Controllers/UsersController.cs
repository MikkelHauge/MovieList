using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using MovieListWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MovieListWebApp.Controllers
{
    public class UsersController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(string sortOrder)
        {

            using (var context = new Context())
            {
                var users = db.Users.Include(u => u.Movies).ToList();

                switch (sortOrder)
                {
                    case "Name":
                        users = users.OrderBy(u => u.Name).ToList();
                        break;
                    case "List":
                        users = users.OrderByDescending(u => u.Movies != null ? u.Movies.Count : 0).ToList();
                        break;
                    case "UserID":
                        users = users.OrderByDescending(u => u.UserId).ToList();
                        break;
                    case "Random":
                        Random random = new Random();
                        users = users.OrderBy(u => random.Next()).ToList();
                        break;
                    default:
                        break;
                }
                UserMovieViewModel viewModel = new UserMovieViewModel();
                viewModel.Users = users;

                ViewBag.SelectedSortMethod = sortOrder;
                return View(viewModel);
            }
        }

        [ChildActionOnly]
        public ActionResult UserChildView(int UserId)
        {

            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);
                User user = userRepo.GetById(UserId);

                if (user == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(user);

                }
            }
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Id = id;
            if (id == null || id <= 0)
            {
                ViewBag.Errormsg = "No ID provided. Redirecting to Movies Index";
                return RedirectToAction("Index");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            List<Movie> nonListedMovies = new List<Movie>();
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);
                user.Movies = movieRepo.GetAllByUserId(user.UserId).ToList();

                nonListedMovies = movieRepo.GetUnlistedMovies(user);

            }
            ViewBag.User = user;
            ViewBag.Nonlisted = nonListedMovies;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Name")] User user)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);

                var userfromDB = userRepo.GetByName(user.Name);

                if (userfromDB != null)
                {
                    ModelState.AddModelError("Name", "User already exists. Usernames must be unique.");
                    return View(user);
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                    }

                }
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                Debug.WriteLine("ModelState errors: " + string.Join(",", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));
                return View(user);
            }

        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
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

        [HttpPost]
        public ActionResult AddMovieToList(int userId, int movieId)
        {
            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);
                var user = userRepo.GetById(userId);
                var movie = movieRepo.GetById(movieId);
                if (user != null && movie != null)
                {
                    user.Movies.Add(movie);
                    userRepo.Update(user);
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

    }
}
