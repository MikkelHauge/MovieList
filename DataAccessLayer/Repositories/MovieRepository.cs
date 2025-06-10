using BusinessLogicLayer.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly Context.Context _context;

        public MovieRepository(Context.Context context)
        {
            _context = context;
        }

        public IQueryable<Movie> GetAll()
        {
            // inklusiv brugerne.
            var movies = _context.Movies.Include(u => u.Users).ToList();
            return movies.AsQueryable();
        }
        public IQueryable<Movie> GetAllWithUsersOnly()
{
             return _context.Movies.Include(u => u.Users.Select(x => x.UserId).ToList());
}
        public List<Movie> GetAllmoviesNoUsers()
        {
                var movies = _context.Movies.ToList();
                return movies;
        }

        public List<Movie> GetAllMoviesContainingName(string name)
        {
            return _context.Movies.Where(u => u.Title.Contains(name)).ToList();
        }
        public Movie GetById(int id)
        {
            // inklusiv brugerne.
            return _context.Movies.Include(m => m.Users).FirstOrDefault(m => m.MovieId == id);
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            _context.Entry(movie).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Movie movie)
        {
            _context.Movies.Attach(movie);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }
        public IQueryable<Movie> GetAllByUserId(int userId)
        {
            return _context.Movies.Where(m => m.Users.Any(u => u.UserId == userId));
        }
        public int GetNumberOfUsersWithMovie(int movieId)
        {
            return _context.Users.Count(u => u.Movies.Any(m => m.MovieId == movieId));
        }
        public List<Movie> GetUnlistedMovies(User user)
        {

            // henter de film, som brugeren IKKE har på sin liste.
            // Bruges i /Users/Details
            // Henter først brugeres liste og derefter de film, som IKKE er på brugerens liste og returnerer den sidstnævnte.
            var usersMovies = _context.Users
                .Where(u => u.UserId == user.UserId)
                .SelectMany(u => u.Movies)
                .Select(m => m.MovieId)
                .ToList();

            var unlistedMovies = _context.Movies
                .Where(m => !usersMovies.Contains(m.MovieId))
                .ToList();

            return unlistedMovies;
        }


    }
}
