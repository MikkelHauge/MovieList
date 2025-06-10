using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_api_test_app.DTO
{
    public class MovieDTO
    {
        public int MovieId { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<int> UserIds { get; set; }
    }
}
