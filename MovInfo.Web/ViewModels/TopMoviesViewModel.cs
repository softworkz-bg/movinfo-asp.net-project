using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class TopMoviesViewModel
    {
        public IList<SingleMovieViewModel> TopMovies { get; set; }
    }
}
