using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovInfo.Services.UnitTests
{
    [TestClass]
    public class MovieServices_Should
    {
        [TestMethod]
        public void AddMovie_Should_Succeed()
        {
            var options = TestUtils.GetOptions(nameof(AddMovie_Should_Succeed));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);
                var result = sut.AddMovieAsync("ExampleName", new DateTime(2019, 12, 12),
                    new List<long> { 1, 2, 3 }, new List<long> { 1, 2, 3 },
                    "https://example.com", "Example Movie Bio", "ExampleImage.jpg",
                    TestSamples.allowedRoles);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);
                Assert.AreEqual(assertContext.Movies.Count(), 1);
            }
        }

        [TestMethod]
        public void AddMovie_Should_AddCorrect_Movie()
        {
            var options = TestUtils.GetOptions(nameof(AddMovie_Should_AddCorrect_Movie));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);
                var result = sut.AddMovieAsync("ExampleName", new DateTime(2019, 12, 12),
                    new List<long> { 1, 2, 3 }, new List<long> { 1, 2, 3 },
                    "https://example.com", "Example Movie Bio", "ExampleImage.jpg",
                    TestSamples.allowedRoles);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Movies.First().Name, "ExampleName");
                Assert.AreEqual(assertContext.Movies.First().DateCreated, new DateTime(2019, 12, 12));
                Assert.AreEqual(assertContext.Movies.First().Trailer, "https://example.com");
                Assert.AreEqual(assertContext.Movies.First().Bio, "Example Movie Bio");
                Assert.AreEqual(assertContext.Movies.First().MainImageName, "ExampleImage.jpg");
                Assert.AreEqual(assertContext.MoviesCategories.First().CategoryId, 1);
                Assert.AreEqual(assertContext.MoviesActors.First().ActorId, 1);            
            }
        }

        [TestMethod]
        public void EditMovie_Should_CorrectlyEdit_Movie()
        {
            var options = TestUtils.GetOptions(nameof(EditMovie_Should_CorrectlyEdit_Movie));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                var editedMovie = sut.EditMovieAsync(
                    TestSamples.exampleMovie.Id,
                    TestSamples.exampleMovie2.Name,
                    TestSamples.exampleMovie2.DateCreated,
                    new List<long> { 1, 2 },
                    new List<long> { 1, 2 },
                    TestSamples.exampleMovie2.Trailer,
                    TestSamples.exampleMovie2.Bio,
                    TestSamples.exampleMovie2.MainImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Movies.First().Id, TestSamples.exampleMovie.Id);
                Assert.AreEqual(assertContext.Movies.First().Name, TestSamples.exampleMovie2.Name);
                Assert.AreEqual(assertContext.Movies.First().DateCreated, TestSamples.exampleMovie2.DateCreated);
                Assert.AreEqual(assertContext.Movies.First().Trailer, TestSamples.exampleMovie2.Trailer);
                Assert.AreEqual(assertContext.Movies.First().Bio, TestSamples.exampleMovie2.Bio);
                Assert.AreEqual(assertContext.Movies.First().MainImageName, TestSamples.exampleMovie2.MainImageName);
                Assert.AreEqual(assertContext.Movies.Include(x => x.MoviesCategories).First().MoviesCategories.Count, 2);
                Assert.AreEqual(assertContext.Movies.Include(x => x.MoviesActors).First().MoviesActors.Count, 2);
            }            
        }

        [TestMethod]
        public void EditMovie_Should_ReturnCorrectlyEdited_Movie()
        {
            var options = TestUtils.GetOptions(nameof(EditMovie_Should_CorrectlyEdit_Movie));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);               

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);

                var editedMovie = sut.EditMovieAsync(
                   TestSamples.exampleMovie.Id,
                   TestSamples.exampleMovie2.Name,
                   TestSamples.exampleMovie2.DateCreated,
                   new List<long> { 1, 2 },
                   new List<long> { 1, 2 },
                   TestSamples.exampleMovie2.Trailer,
                   TestSamples.exampleMovie2.Bio,
                   TestSamples.exampleMovie2.MainImageName,
                    TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(editedMovie, typeof(Movie));
                Assert.AreEqual(editedMovie.Id, TestSamples.exampleMovie.Id);
                Assert.AreEqual(editedMovie.Name, TestSamples.exampleMovie2.Name);
                Assert.AreEqual(editedMovie.DateCreated, TestSamples.exampleMovie2.DateCreated);
                Assert.AreEqual(editedMovie.Trailer, TestSamples.exampleMovie2.Trailer);
                Assert.AreEqual(editedMovie.Bio, TestSamples.exampleMovie2.Bio);
                Assert.AreEqual(editedMovie.MainImageName, TestSamples.exampleMovie2.MainImageName);
                Assert.AreEqual(editedMovie.MoviesCategories.Count, 2);
                Assert.AreEqual(editedMovie.MoviesActors.Count, 2);
            }
        }

        [TestMethod]
        public void DeleteMovie_Should_CorrectlyDelete_Movie()
        {
            var options = TestUtils.GetOptions(nameof(DeleteMovie_Should_CorrectlyDelete_Movie));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);               

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                var deletedMovie = sut.DeleteMovieAsync(
                    TestSamples.exampleMovie.Id,
                    TestSamples.allowedRoles).Result;                

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Movies.Count(), 0);              
            }
        }

        [TestMethod]
        public void DeleteMovie_Should_CorrectlyReturnDeleted_Movie()
        {
            var options = TestUtils.GetOptions(nameof(EditMovie_Should_CorrectlyEdit_Movie));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);

                var deletedMovie = sut.DeleteMovieAsync(
                    TestSamples.exampleMovie.Id,
                    TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(deletedMovie, typeof(Movie));
                Assert.AreEqual(deletedMovie.Id, TestSamples.exampleMovie.Id);
                Assert.AreEqual(deletedMovie.Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(deletedMovie.DateCreated, TestSamples.exampleMovie.DateCreated);
                Assert.AreEqual(deletedMovie.Trailer, TestSamples.exampleMovie.Trailer);
                Assert.AreEqual(deletedMovie.Bio, TestSamples.exampleMovie.Bio);
                Assert.AreEqual(deletedMovie.MainImageName, TestSamples.exampleMovie.MainImageName);
            }
        }

        [TestMethod]
        public void GetMovieWithAllInfo_Should_ReturnCorrectMovie()
        {
            var options = TestUtils.GetOptions(nameof(GetMovieWithAllInfo_Should_ReturnCorrectMovie));

            using (var arrangeContext = new MovInfoContext(options))
            {
                TestSamples.exampleMovie.MoviesCategories.Add(TestSamples.exampleMoviesCategories);
                TestSamples.exampleMovie.MoviesActors.Add(TestSamples.exampleMoviesActors);

                arrangeContext.Movies.Add(TestSamples.exampleMovie);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();

                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);

                var result = sut.GetMovieWithAllInfoAsync(1).Result;

                Assert.IsInstanceOfType(result, typeof(Movie));
                Assert.AreEqual(result.Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.Rating, TestSamples.exampleMovie.Rating);
                Assert.AreEqual(result.Trailer, TestSamples.exampleMovie.Trailer);
                Assert.AreEqual(result.Bio, TestSamples.exampleMovie.Bio);
                Assert.AreEqual(result.MainImageName, TestSamples.exampleMovie.MainImageName);
                Assert.AreEqual(result.MoviesCategories.First().CategoryId, TestSamples.exampleMoviesCategories.CategoryId);
                Assert.AreEqual(result.MoviesActors.First().ActorId, TestSamples.exampleMoviesActors.ActorId);
            }
        }

        [TestMethod]
        public void GetTopEightMoviesOrLess_Should_ReturnCorrectMovies()
        {
            var options = TestUtils.GetOptions(nameof(GetTopEightMoviesOrLess_Should_ReturnCorrectMovies));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var reviewServices = new ReviewServices(arrangeContext, mockBusinessValidator.Object);
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie2 = sut.AddMovieAsync(
                    TestSamples.exampleMovie2.Name,
                    TestSamples.exampleMovie2.DateCreated,
                    new List<long> { 1, 2, 4 },
                    new List<long> { 1, 2, 4 },
                    TestSamples.exampleMovie2.Trailer,
                    TestSamples.exampleMovie2.Bio,
                    TestSamples.exampleMovie2.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie3 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie4 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie5 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie6 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie7 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie8 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie9 = sut.AddMovieAsync(
                   TestSamples.exampleMovie3.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles);


                var addedReview = reviewServices.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleMovie.Id,
                   5,
                   TestSamples.exampleReview.Text,
                   TestSamples.allowedRoles);

                var addedReview2 = reviewServices.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleMovie2.Id,
                   4,
                   TestSamples.exampleReview.Text,
                   TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);
                var result = sut.GetTopEightOrLessMoviesAsync().Result;

                Assert.AreEqual(result.Count, 8);
                Assert.IsInstanceOfType(result.First(), typeof(Movie));

                Assert.AreEqual(result.First().Id, TestSamples.exampleMovie.Id);
                Assert.AreEqual(result.First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.First().DateCreated, TestSamples.exampleMovie.DateCreated);
                Assert.AreEqual(result.First().Trailer, TestSamples.exampleMovie.Trailer);
                Assert.AreEqual(result.First().Bio, TestSamples.exampleMovie.Bio);
                Assert.AreEqual(result.First().MainImageName, TestSamples.exampleMovie.MainImageName);

                Assert.AreEqual(result.Skip(1).First().Id, TestSamples.exampleMovie2.Id);
                Assert.AreEqual(result.Skip(1).First().Name, TestSamples.exampleMovie2.Name);
                Assert.AreEqual(result.Skip(1).First().DateCreated, TestSamples.exampleMovie2.DateCreated);
                Assert.AreEqual(result.Skip(1).First().Trailer, TestSamples.exampleMovie2.Trailer);
                Assert.AreEqual(result.Skip(1).First().Bio, TestSamples.exampleMovie2.Bio);
                Assert.AreEqual(result.Skip(1).First().MainImageName, TestSamples.exampleMovie2.MainImageName);              
            }
        }

        [TestMethod]
        public void GetMoviesStartingWith_Should_ReturnCorrect_Movies()
        {
            var options = TestUtils.GetOptions(nameof(GetMoviesStartingWith_Should_ReturnCorrect_Movies));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie2 = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie2.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie2.Trailer,
                    TestSamples.exampleMovie2.Bio,
                    TestSamples.exampleMovie2.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie3 = sut.AddMovieAsync(
                    TestSamples.exampleMovie5.Name,
                    TestSamples.exampleMovie5.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie5.Trailer,
                    TestSamples.exampleMovie5.Bio,
                    TestSamples.exampleMovie5.MainImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);

                var allMoviesResult = sut.GetMoviesStartingWithAsync('e').Result;

                Assert.IsInstanceOfType(allMoviesResult.First(), typeof(Movie));
                Assert.AreEqual(allMoviesResult.First().Id, TestSamples.exampleMovie.Id);
                Assert.AreEqual(allMoviesResult.First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(allMoviesResult.First().DateCreated, TestSamples.exampleMovie.DateCreated);
                Assert.AreEqual(allMoviesResult.First().Trailer, TestSamples.exampleMovie.Trailer);
                Assert.AreEqual(allMoviesResult.First().Bio, TestSamples.exampleMovie.Bio);
                Assert.AreEqual(allMoviesResult.First().MainImageName, TestSamples.exampleMovie.MainImageName);
                Assert.AreEqual(allMoviesResult.Count(), 2);
            }

        }

        [TestMethod]
        public void ListSixMoviesWithNameAsync_Should_ReturnCorrectMovies()
        {
            var options = TestUtils.GetOptions(nameof(ListSixMoviesWithNameAsync_Should_ReturnCorrectMovies));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles).Result;

                var addedMovie2 = sut.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie2.DateCreated,
                    new List<long> { 1, 2, 4 },
                    new List<long> { 1, 2, 4 },
                    TestSamples.exampleMovie2.Trailer,
                    TestSamples.exampleMovie2.Bio,
                    TestSamples.exampleMovie2.MainImageName,
                    TestSamples.allowedRoles).Result;

                var addedMovie3 = sut.AddMovieAsync(
                   TestSamples.exampleMovie.Name,
                   TestSamples.exampleMovie3.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie3.Trailer,
                   TestSamples.exampleMovie3.Bio,
                   TestSamples.exampleMovie3.MainImageName,
                    TestSamples.allowedRoles).Result;

                var addedMovie4 = sut.AddMovieAsync(
                   TestSamples.exampleMovie.Name,
                   TestSamples.exampleMovie4.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie4.Trailer,
                   TestSamples.exampleMovie4.Bio,
                   TestSamples.exampleMovie4.MainImageName,
                    TestSamples.allowedRoles).Result;

                var addedMovie5 = sut.AddMovieAsync(
                   TestSamples.exampleMovie.Name,
                   TestSamples.exampleMovie5.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie5.Trailer,
                   TestSamples.exampleMovie5.Bio,
                   TestSamples.exampleMovie5.MainImageName,
                    TestSamples.allowedRoles).Result;

                var addedMovie6 = sut.AddMovieAsync(
                   TestSamples.exampleMovie.Name,
                   TestSamples.exampleMovie6.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie6.Trailer,
                   TestSamples.exampleMovie6.Bio,
                   TestSamples.exampleMovie6.MainImageName,
                    TestSamples.allowedRoles);

                var addedMovie7 = sut.AddMovieAsync(
                   TestSamples.exampleMovie7.Name,
                   TestSamples.exampleMovie7.DateCreated,
                   new List<long> { 1, 2, 4 },
                   new List<long> { 1, 2, 4 },
                   TestSamples.exampleMovie7.Trailer,
                   TestSamples.exampleMovie7.Bio,
                   TestSamples.exampleMovie7.MainImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);
                var result = sut.ListSixMoviesWithNameAsync(TestSamples.exampleMovie.Name, null).Result;

                Assert.AreEqual(result.Count, 6);
                Assert.IsInstanceOfType(result.First(), typeof(Movie));

                Assert.AreEqual(result.First().Id, TestSamples.exampleMovie.Id);
                Assert.AreEqual(result.First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.First().DateCreated, TestSamples.exampleMovie.DateCreated);
                Assert.AreEqual(result.First().Trailer, TestSamples.exampleMovie.Trailer);
                Assert.AreEqual(result.First().Bio, TestSamples.exampleMovie.Bio);
                Assert.AreEqual(result.First().MainImageName, TestSamples.exampleMovie.MainImageName);

                Assert.AreEqual(result.Skip(1).First().Id, TestSamples.exampleMovie2.Id);
                Assert.AreEqual(result.Skip(1).First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.Skip(1).First().DateCreated, TestSamples.exampleMovie2.DateCreated);
                Assert.AreEqual(result.Skip(1).First().Trailer, TestSamples.exampleMovie2.Trailer);
                Assert.AreEqual(result.Skip(1).First().Bio, TestSamples.exampleMovie2.Bio);
                Assert.AreEqual(result.Skip(1).First().MainImageName, TestSamples.exampleMovie2.MainImageName);

                Assert.AreEqual(result.Skip(2).First().Id, TestSamples.exampleMovie3.Id);
                Assert.AreEqual(result.Skip(2).First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.Skip(2).First().DateCreated, TestSamples.exampleMovie3.DateCreated);
                Assert.AreEqual(result.Skip(2).First().Trailer, TestSamples.exampleMovie3.Trailer);
                Assert.AreEqual(result.Skip(2).First().Bio, TestSamples.exampleMovie3.Bio);
                Assert.AreEqual(result.Skip(2).First().MainImageName, TestSamples.exampleMovie3.MainImageName);

                Assert.AreEqual(result.Skip(3).First().Id, TestSamples.exampleMovie4.Id);
                Assert.AreEqual(result.Skip(3).First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.Skip(3).First().DateCreated, TestSamples.exampleMovie4.DateCreated);
                Assert.AreEqual(result.Skip(3).First().Trailer, TestSamples.exampleMovie4.Trailer);
                Assert.AreEqual(result.Skip(3).First().Bio, TestSamples.exampleMovie4.Bio);
                Assert.AreEqual(result.Skip(3).First().MainImageName, TestSamples.exampleMovie4.MainImageName);

                Assert.AreEqual(result.Skip(4).First().Id, TestSamples.exampleMovie5.Id);
                Assert.AreEqual(result.Skip(4).First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.Skip(4).First().DateCreated, TestSamples.exampleMovie5.DateCreated);
                Assert.AreEqual(result.Skip(4).First().Trailer, TestSamples.exampleMovie5.Trailer);
                Assert.AreEqual(result.Skip(4).First().Bio, TestSamples.exampleMovie5.Bio);
                Assert.AreEqual(result.Skip(4).First().MainImageName, TestSamples.exampleMovie5.MainImageName);

                Assert.AreEqual(result.Skip(5).First().Id, TestSamples.exampleMovie6.Id);
                Assert.AreEqual(result.Skip(5).First().Name, TestSamples.exampleMovie.Name);
                Assert.AreEqual(result.Skip(5).First().DateCreated, TestSamples.exampleMovie6.DateCreated);
                Assert.AreEqual(result.Skip(5).First().Trailer, TestSamples.exampleMovie6.Trailer);
                Assert.AreEqual(result.Skip(5).First().Bio, TestSamples.exampleMovie6.Bio);
                Assert.AreEqual(result.Skip(5).First().MainImageName, TestSamples.exampleMovie6.MainImageName);             
            }
        }
    }

}


