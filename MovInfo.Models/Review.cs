using System;
using System.ComponentModel.DataAnnotations;

namespace MovInfo.Models
{
    public class Review
    {
        public long Id { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        [MaxLength(150)]
        public string Text { get; set; }

        public long MovieId { get; set; }

        public Movie Movie { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserName { get; set; }
    }
}