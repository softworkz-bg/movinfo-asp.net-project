using Microsoft.EntityFrameworkCore;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly MovInfoContext context;
        private readonly IBusinessLogicValidator businessLogicValidator;

        public MovieServices(MovInfoContext context, IBusinessLogicValidator businessLogicValidator)
        {
            this.context = context;
            this.businessLogicValidator = businessLogicValidator;
        }

        public async Task<Movie> GetMovieWithBasicInfoAsync(long movieId)
        {
            var movieToFind = await context.Movies.FindAsync(movieId);

            businessLogicValidator.IsEntityFound(movieToFind,
               BusinessLogicValidatorMessages.NoSuchMovie);

            return movieToFind;
        }

        public async Task<Movie> GetMovieWithAllInfoAsync(long movieId)
        {
            var movieToFind = await context.Movies.FindAsync(movieId);

            businessLogicValidator.IsEntityFound(movieToFind,
                BusinessLogicValidatorMessages.NoSuchMovie);

            var movieToDisplay = context.Movies
                .Include(m => m.MoviesActors)
                    .Include(m => m.MoviesCategories)
                        .Include(m => m.Reviews)
                            .FirstOrDefault(m => m.Id == movieId);

            return movieToDisplay;
        }

        public async Task<IList<Actor>> GetMovieActorsAsync(long movieId)
        {
            var movieToFind = await context.Movies.FindAsync(movieId);

            businessLogicValidator.IsEntityFound(movieToFind,
               BusinessLogicValidatorMessages.NoSuchMovie);

            var allMovieActors = context.Actors
                .Where(a => a.MoviesActors.Any(ma => ma.MovieId == movieId))
                    .ToList();

            return allMovieActors;
        }

        public async Task<IList<Category>> GetMovieCategoriesAsync(long movieId)
        {
            var movieToFind = await context.Movies.FindAsync(movieId);

            businessLogicValidator.IsEntityFound(movieToFind,
               BusinessLogicValidatorMessages.NoSuchMovie);

            var allMovieCategories = context.Categories
                .Where(c => c.MovieCategories.Any(mc => mc.MovieId == movieId))
                    .ToList();

            return allMovieCategories;
        }

        public async Task<ICollection<Movie>> FindMoviesAsync(string name, string ignoredMovies)
        {
            IQueryable<Movie> SearchList;

            if (!string.IsNullOrEmpty(ignoredMovies))
            {
                var ignoreList = ignoredMovies.Split(',').Select(m => long.Parse(m)).ToArray();
                SearchList = context.Movies.Where(a => !ignoreList.Contains(a.Id)).Select(x => x);
            }
            else
            {
                SearchList = from m in context.Movies
                             select m;
            }

            if (!string.IsNullOrEmpty(name))
            {
                SearchList = SearchList.Where(s => s.Name.Contains(name));
            }

            return await SearchList.ToListAsync();
        }

        public async Task<ICollection<Movie>> GetTopEightOrLessMoviesAsync()
        {
            var topEightMovies = context.Movies.OrderByDescending(x => x.Rating).Take(8);

            businessLogicValidator.AreAnyEntitiesPresent(topEightMovies,
                BusinessLogicValidatorMessages.NoMoviesInApp);

            await context.SaveChangesAsync();

            return await topEightMovies.ToListAsync();
        }

        public async Task<IReadOnlyCollection<Movie>> GetMoviesStartingWithAsync(char symbol)
        {
            var movies = await this.context.Movies
                                     .Where(t => t.Name.ToLower().StartsWith(symbol.ToString().ToLower()))
                                     .ToListAsync();

            return movies;
        }

        public async Task<IReadOnlyCollection<Movie>> ListSixMoviesWithNameAsync(string name, int? pageNo)
        {
            List<Movie> movies;
            if (pageNo == null || pageNo == 1)
            {
                movies = await this.context.Movies
                             .Where(t => t.Name.Contains(name))
                             .Take(6)
                             .ToListAsync();
            }
            else
            {
                movies = await this.context.Movies
                            .Where(t => t.Name.Contains(name))
                            .Skip(((int)pageNo - 1) * 6)
                            .Take(6)
                            .ToListAsync();
            }

            return movies;
        }

        public async Task<IReadOnlyCollection<Movie>> GetMoviesByRatingAsync()
        {
            var movies = await context.Movies
                           .OrderByDescending(ar => ar.Rating)
                           .ToListAsync();

            return movies;
        }

        public async Task<Movie> EditMovieAsync(
            long movieId,
            string movieName,
            DateTime movieDate,
            IEnumerable<long> categoriesIds,
            IEnumerable<long> actorsIds,
            string trailerLink,
            string movieBio,
            string mainImageName,
            ICollection<string> allowedRoles)
        {
            businessLogicValidator.IsAnotherMovieAlreadyPresent(context, movieName, movieDate.Year, movieId);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var movieToEdit = await context.Movies
                .Include(m => m.MoviesActors)
                    .Include(m => m.MoviesCategories)
                        .Include(m => m.Reviews)
                            .FirstOrDefaultAsync(m => m.Id == movieId);

            movieToEdit.Name = movieName;
            movieToEdit.DateCreated = movieDate;
            movieToEdit.Trailer = trailerLink;
            movieToEdit.Bio = movieBio;
            movieToEdit.MainImageName = mainImageName;

            var actorMovieRecords = actorsIds
              .Select(actor => new MoviesActors() { ActorId = actor })
                  .ToList();

            var categoryMovieRecords = categoriesIds
              .Select(categoryId => new MoviesCategories() { CategoryId = categoryId })
                  .ToList();          

           context.TryUpdateManyToMany(movieToEdit.MoviesActors, actorMovieRecords
            .Select(x => new MoviesActors
            {
                ActorId = x.ActorId,
                MovieId = movieId   
            }), x => x.ActorId);

           context.TryUpdateManyToMany(movieToEdit.MoviesCategories, categoryMovieRecords
           .Select(x => new MoviesCategories
           {
               CategoryId = x.CategoryId,
               MovieId = movieId
           }), x => x.CategoryId);      

            await context.SaveChangesAsync();

            return movieToEdit;
       }


        public async Task<Movie> AddMovieAsync(
            string movieName,
            DateTime movieDate,
            IEnumerable<long> categoriesIds,
            IEnumerable<long> actorsIds,
            string trailerLink,
            string movieBio,
            string mainImageName,
            ICollection<string> allowedRoles)
        {
            businessLogicValidator.IsMovieAlreadyPresent(context, movieName, movieDate.Year);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);


            var movieToAdd = new Movie
            {
                Name = movieName,
                DateCreated = movieDate,
                Trailer = trailerLink,
                Bio = movieBio,
                MainImageName = mainImageName
            };

            movieToAdd.MoviesCategories = categoriesIds
               .Select(category => new MoviesCategories() { CategoryId = category })
                   .ToList();

            movieToAdd.MoviesActors = actorsIds
               .Select(actor => new MoviesActors() { ActorId = actor })
                   .ToList();

            await context.Movies
               .AddAsync(movieToAdd);

            await context.SaveChangesAsync();

            return movieToAdd;
        }       

        public async Task<Movie> DeleteMovieAsync(long movieId, ICollection<string> allowedRoles)
        {
            businessLogicValidator.DoesMovieExist(context, movieId);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var movieToDelete = await context.Movies.FindAsync(movieId);

            context.Movies.Remove(movieToDelete);

            await context.SaveChangesAsync();

            return movieToDelete;
        }
    }
}
