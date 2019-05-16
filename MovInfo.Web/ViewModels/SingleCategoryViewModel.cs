using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class SingleCategoryViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public IList<SingleMovieViewModel> AllMovies { get; set; }

        public string AllMoviesIdsString { get; set; }

        public bool CanUserEdit { get; set; }

        public bool CanUserDelete { get; set; }
    }
}
