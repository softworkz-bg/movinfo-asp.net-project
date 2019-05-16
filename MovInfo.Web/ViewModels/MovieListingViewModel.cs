using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class MovieListingViewModel
    {
        public IList<SingleMovieViewModel> SixMoviesWithName { get; set; }

        public int TotalPagesCount { get; set; }

        public int TargetPage { get; set; }

        public string SearchName { get; set; }
    }
}
