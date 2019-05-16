using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Services.UnitTests
{
    public static class TestSamples
    {
        public static long oldReviewRating = 5;

        public static string[] allowedRoles = new string[] { "Manager, Admin" };

        public static Review exampleReview = new Review()
        {
            Id = 1,
            Rating = 9,
            Text = "ExampleRw",
            MovieId = 1,
            ApplicationUserName = "ExampleName",
            ApplicationUserId = new Guid().ToString()
        };

        public static Review exampleReview2 = new Review()
        {
            Id = 2,
            Rating = 4,
            Text = "ExampleRw2",
            MovieId = 1,
            ApplicationUserId = new Guid().ToString()
        };

        public static Actor exampleActor = new Actor()
        {
            Id = 1,
            FirstName = "ExampleFirstName",
            LastName = "ExampleLastName",
            Bio = "Example Actor Bio",
            MoviesActors = new List<MoviesActors>(),
            ProfileImageName = "ExampleName.jpg"
        };

        public static Actor exampleActor2 = new Actor()
        {
            Id = 2,
            FirstName = "ExampleFirstName",
            LastName = "ExampleLastName",
            Bio = "Example Actor Bio",
            MoviesActors = new List<MoviesActors>(),
            ProfileImageName = "ExampleName.jpg"            
        };


        public static Movie exampleMovie = new Movie()
        {
            Id = 1,
            Name = "ExampleName",
            Rating = 8,
            DateCreated = new DateTime(2019, 01, 01),
            Trailer = "https://example.com",
            Bio = "Example Movie Bio",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage.jpg"

        };

        public static Movie exampleMovie2 = new Movie()
        {
            Id = 2,
            Name = "ExampleName2",
            Rating = 7,
            DateCreated = new DateTime(2000, 11, 11),
            Trailer = "https://exampl2e.com",
            Bio = "Example Movie Bio2",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage2.jpg"

        };

        public static Movie exampleMovie3 = new Movie()
        {
            Id = 3,
            Name = "ExampleName",
            Rating = 9,
            DateCreated = new DateTime(2001, 12, 12),
            Trailer = "https://example.com",
            Bio = "Example Movie Bio",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage.jpg"

        };

        public static Movie exampleMovie4 = new Movie()
        {
            Id = 4,
            Name = "ExampleName",
            Rating = 2,
            DateCreated = new DateTime(1997, 10, 10),
            Trailer = "https://example.com",
            Bio = "Example Movie Bio",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage.jpg"

        };

        public static Movie exampleMovie5 = new Movie()
        {
            Id = 5,
            Name = "NotSameName",
            Rating = 2,
            DateCreated = new DateTime(1997, 10, 10),
            Trailer = "https://example.com",
            Bio = "Example Movie Bio",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage.jpg"

        };

        public static Movie exampleMovie6 = new Movie()
        {
            Id = 6,
            Name = "NotSameName",
            Rating = 2,
            DateCreated = new DateTime(1997, 10, 10),
            Trailer = "https://example.com",
            Bio = "Example Movie Bio",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage.jpg"

        };

        public static Movie exampleMovie7 = new Movie()
        {
            Id = 7,
            Name = "NotSameName7",
            Rating = 2,
            DateCreated = new DateTime(1998, 10, 10),
            Trailer = "https://example2.com",
            Bio = "Example Movie Bio7",
            MoviesCategories = new List<MoviesCategories>(),
            MoviesActors = new List<MoviesActors>(),
            MainImageName = "ExampleImage7.jpg"

        };

        public static Category exampleCategory = new Category()
        {
            Id = 1,
            Title = "ExampleTitle",
            MovieCategories = new List<MoviesCategories>()
        };

        public static Category exampleCategory2 = new Category()
        {
            Id = 2,
            Title = "ExampleTitle2",
            MovieCategories = new List<MoviesCategories>()
        };

        public static MoviesCategories exampleMoviesCategories = new MoviesCategories()
        {
            CategoryId = 1,
            MovieId = 1
        };

        public static MoviesActors exampleMoviesActors = new MoviesActors()
        {
            ActorId = 1,
            MovieId = 1
        };
    }
}
