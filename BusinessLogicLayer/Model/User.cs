using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogicLayer.Model
{
    public class User
    {
        public int UserId { get; set; }


        [Required]
        [StringLength(60)]
        [Index(IsUnique = true)]
        [RegularExpression("^[a-zA-Z0-9ÆØÅæøå ]*$", ErrorMessage = "Username can only contain letters, numbers, and spaces.")]
        public string Name { get; set; }

        public List<Movie> Movies { get; set; }


        // bruges kun til web api test appen
        public string nameAndId
        {
            get { return "" + UserId + " " + Name; }
        }
    }
}
