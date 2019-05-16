using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class SingleReviewViewModel
    {
        public long Id { get; set; }

        [Range(1, 5)]
        [Required(ErrorMessage = "Rating is Required!")]
        public double Rating { get; set; }

        public double OldRating { get; set; }

        [MinLength(20)]
        [MaxLength(150)]
        [Required(ErrorMessage = "Review must be between 20 and 150 characters")]
        public string Text { get; set; }

        public long MovieId { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserId { get; set; }

        public bool CanUserEdit { get ; set; }

        public bool CanUserDelete { get; set; }
    }
}
