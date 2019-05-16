using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovInfo.Models
{
    public class Movie
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public double Rating { get; set; }

        public double AllRatingsSum { get; set; }

        public int TotalRatings { get; set; }

        public ICollection<MoviesCategories> MoviesCategories { get; set; }

        public ICollection<MoviesActors> MoviesActors { get; set; }

        public IList<Review> Reviews { get; set; }

        [Required]
        public string Trailer { get; set; }

        [Required]
        [MaxLength(150)]
        public string Bio { get; set; }

        public string MainImageName { get; set; }
    }

}
