namespace MovInfo.Models
{
    public class MoviesActors
    {
        public long MovieId { get; set; }
        public Movie Movie { get; set; }

        public long ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
