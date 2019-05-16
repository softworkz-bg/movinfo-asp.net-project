using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovInfo.Models
{
    public class Category
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<MoviesCategories> MovieCategories { get; set; }
    }
}
