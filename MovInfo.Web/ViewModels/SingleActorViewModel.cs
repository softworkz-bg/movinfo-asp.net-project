using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovInfo.Web.ViewModels
{
    public class SingleActorViewModel
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(150)]
        public string Bio { get; set; }

        public string FullImagePath { get; set; }

        public IList<SingleMovieViewModel> AllMovies { get; set; }

        public IFormFile MainImage { get; set; }

        public string MainImageName { get; set; }

        public bool CanUserEdit { get; set; }

        public bool CanUserDelete { get; set; }
    }
}
