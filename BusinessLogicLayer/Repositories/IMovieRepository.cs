using BusinessLogicLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public interface IMovieRepository
    {
        IQueryable<Movie> GetAll();
        Movie GetById(int id);

        IQueryable<Movie> GetAllWithUsersOnly();

        void Add(Movie movie);
        void Update(Movie movie);
        void Delete(Movie movie);
        IQueryable<Movie> GetAllByUserId(int userId);
        int GetNumberOfUsersWithMovie(int movieId);
        List<Movie> GetUnlistedMovies(User user);
    }
}
