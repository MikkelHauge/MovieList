using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    // DTO klasserne bruges til at undgå et uendeligt langt loop af JSON kald, fordi film indeholder brugere, og brugere indeholder film, osv.
    // så deres referencer er bare en liste af ints (id's)
    public class MovieDTO
    {
        public int MovieId { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<int> UserIds { get; set; }
    }
}
