using BusinessLogicLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_api_test_app.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<int> MovieIds { get; set; }


        public static User ConvertUserDTOToUser(List<UserDTO> userDTOs)
        {
            if (userDTOs == null || userDTOs.Count == 0) return null;

            var userDTO = userDTOs[0];

            var user = new User
            {
                UserId = userDTO.UserId,
                Name = userDTO.Name,
                Movies = new List<Movie>()
            };

            foreach (var movieId in userDTO.MovieIds)
            {
                user.Movies.Add(new Movie { MovieId = movieId });
            }

            return user;
        }
    }
}
