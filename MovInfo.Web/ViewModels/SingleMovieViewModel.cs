using Microsoft.AspNetCore.Http;
using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class SingleMovieViewModel
    {      
        public long Id { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Release date is required!")]
        [DataType(DataType.Date)]        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateCreated { get; set; }

        public double Rating { get; set; }

        public int NumberOfRatings { get; set; }

        public IList<SingleCategoryViewModel> AllCategories { get; set; }

        public IList<SingleActorViewModel> AllActors { get; set; }

        public IList<SingleReviewViewModel> AllReviews { get; set; }

        [Required(ErrorMessage = "One or more Categories are required!")]
        public string AllCategoriesIdsString { get; set; }

        [Required(ErrorMessage = "One or more Actors are required!")]
        public string AllActorsIdsString { get; set; }

        [Url(ErrorMessage = "Please put valid Url!")]
        [Required(ErrorMessage = "Trailer Url is required!")]        
        public string Trailer { get; set; }

        [Required(ErrorMessage = "Biography is required and between 10 and 250 symbols!")]
        [MinLength(10)]
        [MaxLength(250)]
        public string Bio { get; set; }

        public string FullImagePath { get; set; }

        public string MainImageName { get; set; }

        [Required(ErrorMessage = "Poster for Movie is required!")]
        public IFormFile MainImage { get; set; }

        public SingleReviewViewModel CurrentReview { get; set; }

        public bool CanUserEdit { get; set; }

        public TopMoviesViewModel TopEightMovies { get; set; }
    }
}
