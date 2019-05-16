using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovInfo.Models
{
    public class Actor
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

        public string ProfileImageName { get; set; }

        public ICollection<MoviesActors> MoviesActors { get; set; }
    }
}