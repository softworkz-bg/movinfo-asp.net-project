using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class MovieViewModel
    {
        public IDictionary<long, string> AllMoviesIdsAndNames { get; set; }

        public ICollection<long> MoviesIds { get; set; }
    }
}
