using BusinessLogicLayer.Model;
using System.Collections.Generic;

namespace MovieListWebApp.Models
{
    public class UserMovieViewModel
    {
        public List<User> Users { get; set; }
        public List<Movie> Movies { get; set; }
    }
}