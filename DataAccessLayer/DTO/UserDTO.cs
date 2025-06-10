using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class UserDTO
    {
        // DTO klasserne bruges til at undgå et uendeligt langt loop af JSON kald, fordi film indeholder brugere, og brugere indeholder film, osv.
        // så deres referencer er bare en liste af ints (id's)
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<int> MovieIds { get; set; }

    }

}
