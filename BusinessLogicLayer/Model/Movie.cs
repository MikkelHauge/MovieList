using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BusinessLogicLayer.Model
{
    public class Movie
    {
        public int MovieId { get; set; }



        [Required]
        [Index("IX_ReleaseYear_Title", 1, IsUnique = true)]
        [Range(1850, 2100, ErrorMessage = "Release year must be between 1850 and 2100. 🤷‍♂️")]
        public int ReleaseYear { get; set; }



        [Required]
        [StringLength(100)]
        [Index("IX_ReleaseYear_Title", 2, IsUnique = true)]
        public string Title { get; set; }
        public string Description { get; set; }
        public List<User> Users { get; set; }

        public string TitleAndYear
        {
            get { return Title + " (" + ReleaseYear + ")"; }
        }

        public int UsersCount
        {
            get { return this.Users != null ? this.Users.Count : 0; }
        }




        // bruges kun til wpf test api appen
        public string TitleAndId
        {
            get { return "" + MovieId + " " + Title; }
        }

    }
}
