using BusinessLogicLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Context
{
    internal class DBinitialiser: CreateDatabaseIfNotExists<Context>
    {
        public DBinitialiser() { }
        protected override void Seed(Context context)
        {
            base.Seed(context);
            try
            {
                // Initial data:

                // Movies
                Movie movie1 = new Movie
                {
                    Title = "John Wick",
                    Description = "They killed his dog",
                    ReleaseYear = 2014,
                    Users = new List<User> { }
                };

                Movie movie2 = new Movie
                {
                    Title = "The Matrix",
                    Description = "They killed his mom",
                    ReleaseYear = 1999,
                    Users = new List<User> { }
                };

                Movie movie3 = new Movie
                {
                    Title = "Back to the Future",
                    Description = "A teenager travels through time in a DeLorean car.",
                    ReleaseYear = 1985,
                    Users = new List<User> { }
                };

                Movie movie4 = new Movie
                {
                    Title = "The Shawshank Redemption",
                    Description = "A man's journey through the prison system after being wrongfully convicted.",
                    ReleaseYear = 1994,
                    Users = new List<User> { }
                };

                Movie movie5 = new Movie
                {
                    Title = "Pulp Fiction",
                    Description = "Interconnected stories of crime, violence, and redemption.",
                    ReleaseYear = 1994,
                    Users = new List<User> { }
                };

                Movie movie6 = new Movie
                {
                    Title = "The Dark Knight",
                    Description = "Batman faces off against the Joker in a battle for Gotham City.",
                    ReleaseYear = 2008,
                    Users = new List<User> { }
                };

                Movie movie7 = new Movie
                {
                    Title = "Inception",
                    Description = "A thief who steals corporate secrets through dream-sharing technology.",
                    ReleaseYear = 2010,
                    Users = new List<User> { }
                };

                Movie movie8 = new Movie
                {
                    Title = "Interstellar",
                    Description = "A group of explorers traveling through a wormhole in search of a new home for humanity.",
                    ReleaseYear = 2014,
                    Users = new List<User> { }
                };

                Movie movie9 = new Movie
                {
                    Title = "The Avengers",
                    Description = "Superheroes join forces to protect the world from a powerful enemy.",
                    ReleaseYear = 2012,
                    Users = new List<User> { }
                };

                Movie movie10 = new Movie
                {
                    Title = "The Social Network",
                    Description = "The story of the creation of Facebook and its subsequent controversies.",
                    ReleaseYear = 2010,
                    Users = new List<User> { }
                };

                Movie movie11 = new Movie
                {
                    Title = "Get Out",
                    Description = "A young African-American man visits his white girlfriend's family, only to uncover disturbing secrets.",
                    ReleaseYear = 2017,
                    Users = new List<User> { }
                };

                Movie movie12 = new Movie
                {
                    Title = "Parasite",
                    Description = "The story of two families from different socioeconomic backgrounds intertwining in unexpected ways.",
                    ReleaseYear = 2019,
                    Users = new List<User> { }
                };

                // Users
                User user1 = new User
                {
                    Name = "Torben",
                    Movies = new List<Movie> { movie1, movie2 }
                };

                User user2 = new User
                {
                    Name = "Margrethe",
                    Movies = new List<Movie> { movie3, movie4, movie5 }
                };

                User user3 = new User
                {
                    Name = "Peter",
                    Movies = new List<Movie> { movie6, movie7 }
                };

                User user4 = new User
                {
                    Name = "Søren",
                    Movies = new List<Movie> { movie8, movie9 }
                };

                User user5 = new User
                {
                    Name = "Hanne",
                    Movies = new List<Movie> { movie10, movie11 }
                };

                User user6 = new User
                {
                    Name = "Kaj",
                    Movies = new List<Movie> { movie2, movie3, movie4 }
                };

                User user7 = new User
                {
                    Name = "Mikkel",
                    Movies = new List<Movie> { movie5, movie6, movie7 }
                };

                
                context.Movies.Add(movie1);
                context.Movies.Add(movie2);
                context.Movies.Add(movie3);
                context.Movies.Add(movie4);
                context.Movies.Add(movie5);
                context.Movies.Add(movie6);
                context.Movies.Add(movie7);
                context.Movies.Add(movie8);
                context.Movies.Add(movie9);
                context.Movies.Add(movie10);
                context.Movies.Add(movie11);
                context.Movies.Add(movie12);
                context.Users.Add(user1);
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.Users.Add(user3);
                context.Users.Add(user4);
                context.Users.Add(user5);
                context.Users.Add(user6);
                context.Users.Add(user7);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // do nothing
            } 
        }
    }
}
