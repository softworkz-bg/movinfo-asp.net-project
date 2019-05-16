using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovInfo.Data;
using MovInfo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services
{
    public static class ProviderServices
    {
        public static async Task SeedDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MovInfoContext>();

                var actorsAsJson = await File.ReadAllTextAsync(@"..\MovInfo.Data\JsonRaw\actors.json");
                var actorMoviesAsJson = await File.ReadAllTextAsync(@"..\MovInfo.Data\JsonRaw\moviesActors.json");
                var categoriesAsJson = await File.ReadAllTextAsync(@"..\MovInfo.Data\JsonRaw\categories.json");
                var moviesAsJson = await File.ReadAllTextAsync(@"..\MovInfo.Data\JsonRaw\movies.json");
                var movieCategoriesAsJson = await File.ReadAllTextAsync(@"..\MovInfo.Data\JsonRaw\moviesCategories.json");

                var actors = JsonConvert.DeserializeObject<Actor[]>(actorsAsJson);
                var actorMovies = JsonConvert.DeserializeObject<MoviesActors[]>(actorMoviesAsJson);
                var movies = JsonConvert.DeserializeObject<Movie[]>(moviesAsJson);
                var movieCategories = JsonConvert.DeserializeObject<MoviesCategories[]>(movieCategoriesAsJson);
                var categories = JsonConvert.DeserializeObject<Category[]>(categoriesAsJson);

                var builder = new StringBuilder();
   
                foreach (var movie in movies)
                {
                    builder.AppendLine($@"
                    
                    IF NOT EXISTS (SELECT * 
                                    FROM dbo.Movies m
                                    WHERE m.Id = {movie.Id})
                        BEGIN
                        INSERT INTO dbo.Movies
                             (Name, DateCreated, Rating, AllRatingsSum, TotalRatings, Trailer, Bio, MainImageName) 
                        VALUES ('{movie.Name}', '{movie.DateCreated}', '{movie.Rating}', '{movie.AllRatingsSum}', '{movie.TotalRatings}', '{movie.Trailer}', '{movie.Bio}', '{movie.MainImageName}')
                        END
                    ");
                }

                foreach (var actor in actors)
                {
                    builder.AppendLine($@"
                    
                    IF NOT EXISTS (SELECT * 
                                    FROM dbo.Actors a
                                    WHERE a.Id = {actor.Id})
                        BEGIN
                        INSERT INTO dbo.Actors
                             (FirstName, LastName, Bio, ProfileImageName) 
                        VALUES ('{actor.FirstName}', '{actor.LastName}', '{actor.Bio}', '{actor.ProfileImageName}')
                        END
                    ");
                }

                foreach (var category in categories)
                {
                    builder.AppendLine($@"
                    
                    IF NOT EXISTS (SELECT * 
                                    FROM dbo.Categories c
                                    WHERE c.Id = {category.Id})
                        BEGIN
                        INSERT INTO dbo.Categories
                             (Title) 
                        VALUES ('{category.Title}')
                        END
                    ");
                }

                foreach (var movCat in movieCategories)
                {
                    builder.AppendLine($@"
                        IF NOT EXISTS (SELECT * 
                                        FROM dbo.MoviesCategories mc
                                        WHERE mc.MovieId = {movCat.MovieId} AND mc.CategoryId = {movCat.CategoryId})
                            BEGIN
                            INSERT INTO dbo.MoviesCategories
                                 (MovieId, CategoryId) 
                            VALUES ('{movCat.MovieId}', '{movCat.CategoryId}')
                            END
                        ");
                }

                foreach (var movActor in actorMovies)
                {
                    builder.AppendLine($@"
                    
                    IF NOT EXISTS (SELECT * 
                                    FROM dbo.MoviesActors ma
                                    WHERE ma.MovieId = {movActor.MovieId} AND ma.ActorId = {movActor.ActorId})
                        BEGIN
                        INSERT INTO dbo.MoviesActors
                             (MovieId, ActorId) 
                        VALUES ('{movActor.MovieId}', '{movActor.ActorId}')
                        END
                    ");
                }

                dbContext.Database.ExecuteSqlCommand(builder.ToString());
            }
        }
    }
}
