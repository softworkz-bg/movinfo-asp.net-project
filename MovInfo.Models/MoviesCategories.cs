namespace MovInfo.Models
{
    public class MoviesCategories
    {
        public long MovieId { get; set; }
        public Movie Movie { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
