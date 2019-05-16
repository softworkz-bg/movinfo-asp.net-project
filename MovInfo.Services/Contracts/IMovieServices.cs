using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services.Contracts
{
    public interface IMovieServices
    {
        Task<Movie> GetMovieWithBasicInfoAsync(long movieId);

        Task<Movie> GetMovieWithAllInfoAsync(long movieId);

        Task<IList<Actor>> GetMovieActorsAsync(long movieId);

        Task<IList<Category>> GetMovieCategoriesAsync(long movieId);

        Task<ICollection<Movie>> FindMoviesAsync(string name, string ignoredMovies);

        Task<ICollection<Movie>> GetTopEightOrLessMoviesAsync();

        Task<IReadOnlyCollection<Movie>> GetMoviesStartingWithAsync(char symbol);

        Task<IReadOnlyCollection<Movie>> ListSixMoviesWithNameAsync(string name, int? pageNo);

        Task<IReadOnlyCollection<Movie>> GetMoviesByRatingAsync();

        Task<Movie> DeleteMovieAsync(long movieId, ICollection<string> allowedRoles);

        Task<Movie> EditMovieAsync(
            long movieId,
            string movieName,
            DateTime movieDate,
            IEnumerable<long> categoriesIds,
            IEnumerable<long> actorsIds,
            string trailerLink,
            string movieBio,
            string mainImageName,
            ICollection<string> allowedRoles);


        Task<Movie> AddMovieAsync(
            string movieName,
            DateTime movieDate,
            IEnumerable<long> categoriesIds,
            IEnumerable<long> actorsIds,
            string trailerLink,
            string movieBio,
            string mainImageName,
             ICollection<string> allowedRoles);        
    }
}
