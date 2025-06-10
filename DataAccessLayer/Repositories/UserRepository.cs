using BusinessLogicLayer.Model;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BusinessLogicLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users.Include(u => u.Movies);
        }

        public User GetByName(string name)
        {
            return _context.Users.Include(u => u.Movies).FirstOrDefault(u => u.Name == name);
        }

        public List<User> GetAllUsersContainingName(string name)
        {
            return _context.Users.Where(u => u.Name.Contains(name)).ToList();
        }
    

        public User GetById(int id)
        {
            return _context.Users.Include(m => m.Movies).FirstOrDefault(u => u.UserId == id);
        }

        public void Add(User user)
        {
            User newUser = GetByName(user.Name);
            if (newUser != null)
            {
                // findes brugerens navn allerede?
                // gør ingenting (databasen tager ikke i mod dubletter af navne, men jeg ville gerne lave dobbelt-tjek)
            }
            else
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                } catch
                {
                    // do nothing (fejl)
                }               

            }
        }

        public void Update(User user)
        {
            using (var context = new Context())
            {
                var dbUser = context.Users.Include(u => u.Movies).Single(u => u.UserId == user.UserId);

                dbUser.Name = user.Name;

                foreach (var movie in user.Movies)
                {
                    if (!dbUser.Movies.Any(m => m.MovieId == movie.MovieId))
                    {
                        dbUser.Movies.Add(context.Movies.Find(movie.MovieId));
                    }
                }

                foreach (var movie in dbUser.Movies.ToList())
                {
                    if (!user.Movies.Any(m => m.MovieId == movie.MovieId))
                    {
                        dbUser.Movies.Remove(movie);
                    }
                }

                context.SaveChanges();
            }
        }


        public void Delete(User user) 
        { 
            using (var context = new Context())
            {
                _context.Users.Attach(user);
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
