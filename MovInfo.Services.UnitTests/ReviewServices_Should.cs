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
    public class ReviewServices_Should
    {
        [TestMethod]
        public void AddReview_Should_Succeed()
        {
            var options = TestUtils.GetOptions(nameof(AddReview_Should_Succeed));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);
                var result = sut.AddReviewAsync(
                    TestSamples.exampleReview.ApplicationUserName, 
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview.Rating,
                    TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);                

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                Assert.AreEqual(assertContext.Reviews.Count(), 1);
            }
        }

        [TestMethod]
        public void AddReview_Should_CorrectlyAssignReview()
        {
            var options = TestUtils.GetOptions(nameof(AddReview_Should_CorrectlyAssignReview));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);

                var result = sut.AddReviewAsync(
                    TestSamples.exampleReview.ApplicationUserName,
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview.Rating,
                    TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                Assert.AreEqual(assertContext.Reviews.First().Id, TestSamples.exampleReview.Id);
                Assert.AreEqual(assertContext.Reviews.First().MovieId, TestSamples.exampleReview.MovieId);
                Assert.AreEqual(assertContext.Reviews.First().Rating, TestSamples.exampleReview.Rating);
                Assert.AreEqual(assertContext.Reviews.First().Text, TestSamples.exampleReview.Text);
                Assert.AreEqual(assertContext.Reviews.First().ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
                Assert.AreEqual(assertContext.Reviews.First().ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
            }
        }

        [TestMethod]
        public void AddReview_Should_CorrectlyReturnReview()
        {
            var options = TestUtils.GetOptions(nameof(AddReview_Should_CorrectlyReturnReview));

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
                var sut = new ReviewServices(assertContext, mockBusinessValidator.Object);

                var addedReview = sut.AddReviewAsync(
                     TestSamples.exampleReview.ApplicationUserName,
                     TestSamples.exampleReview.ApplicationUserId,
                     TestSamples.exampleReview.MovieId,
                     TestSamples.exampleReview.Rating,
                     TestSamples.exampleReview.Text,
                   TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(addedReview, typeof(Review));
                Assert.AreEqual(addedReview.Id, 1);
                Assert.AreEqual(addedReview.MovieId, TestSamples.exampleReview.MovieId);
                Assert.AreEqual(addedReview.Rating, TestSamples.exampleReview.Rating);
                Assert.AreEqual(addedReview.Text, TestSamples.exampleReview.Text);
                Assert.AreEqual(addedReview.ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
                Assert.AreEqual(addedReview.ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
            }
        }

        [TestMethod]
        public void GetReviewAsync_Should_ReturnCorrectlyAssignedReview()
        {
            var options = TestUtils.GetOptions(nameof(GetReviewAsync_Should_ReturnCorrectlyAssignedReview));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);

                var result = sut.AddReviewAsync(
                    TestSamples.exampleReview.ApplicationUserName,
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview.Rating,
                    TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(assertContext, mockBusinessValidator.Object);
                var result = sut.GetReviewAsync(TestSamples.exampleReview.Id).Result;

                Assert.IsInstanceOfType(result, typeof(Review));
                Assert.AreEqual(result.Id, TestSamples.exampleReview.Id);
                Assert.AreEqual(result.MovieId, TestSamples.exampleReview.MovieId);
                Assert.AreEqual(result.Rating, TestSamples.exampleReview.Rating);
                Assert.AreEqual(result.Text, TestSamples.exampleReview.Text);
                Assert.AreEqual(result.ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
                Assert.AreEqual(result.ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
            }
        }

        [TestMethod]
        public void UpdateReviewAsync_Should_CorrectlyUpdateReview()
        {
            var options = TestUtils.GetOptions(nameof(UpdateReviewAsync_Should_CorrectlyUpdateReview));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);

                var addedReview = sut.AddReviewAsync(
                    TestSamples.exampleReview.ApplicationUserName,
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview.Rating,
                    TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                var result = sut.UpdateReviewAsync(
                    TestSamples.exampleReview.Id,
                    TestSamples.exampleReview.ApplicationUserName,
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview2.Rating,
                    TestSamples.oldReviewRating,
                    TestSamples.exampleReview2.Text,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                Assert.AreEqual(assertContext.Reviews.First().Id, TestSamples.exampleReview.Id);
                Assert.AreEqual(assertContext.Reviews.First().MovieId, TestSamples.exampleReview.MovieId);
                Assert.AreEqual(assertContext.Reviews.First().Rating, TestSamples.exampleReview2.Rating);
                Assert.AreEqual(assertContext.Reviews.First().Text, TestSamples.exampleReview2.Text);
                Assert.AreEqual(assertContext.Reviews.First().ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
                Assert.AreEqual(assertContext.Reviews.First().ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
            }
        }

        [TestMethod]
        public void UpdateReview_Should_ReturnCorrectlyUpdatedReview()
        {
            var options = TestUtils.GetOptions(nameof(UpdateReview_Should_ReturnCorrectlyUpdatedReview));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);
                var movieServices = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = movieServices.AddMovieAsync(
                  TestSamples.exampleMovie.Name,
                  TestSamples.exampleMovie.DateCreated,
                  new List<long> { 1, 2, 3 },
                  new List<long> { 1, 2, 3 },
                  TestSamples.exampleMovie.Trailer,
                  TestSamples.exampleMovie.Bio,
                  TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                var addedReview = sut.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleReview.MovieId,
                   TestSamples.exampleReview.Rating,
                   TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(assertContext, mockBusinessValidator.Object);

                var result = sut.UpdateReviewAsync(
                    TestSamples.exampleReview.Id,
                    TestSamples.exampleReview.ApplicationUserName,
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview2.Rating,
                    TestSamples.oldReviewRating,
                    TestSamples.exampleReview2.Text,
                    TestSamples.allowedRoles).Result;

                Assert.AreEqual(result.Id, TestSamples.exampleReview.Id);
                Assert.AreEqual(result.MovieId, TestSamples.exampleReview.MovieId);
                Assert.AreEqual(result.Rating, TestSamples.exampleReview2.Rating);
                Assert.AreEqual(result.Text, TestSamples.exampleReview2.Text);
                Assert.AreEqual(result.ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
                Assert.AreEqual(result.ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
            }
        }

        [TestMethod]
        public void UpdateReview_Should_CorrectlyUpdateMovieRating()
        {
            var options = TestUtils.GetOptions(nameof(UpdateReview_Should_CorrectlyUpdateMovieRating));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);
                var movieServices = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = movieServices.AddMovieAsync(
                    TestSamples.exampleMovie.Name,
                    TestSamples.exampleMovie.DateCreated,
                    new List<long> { 1, 2, 3 },
                    new List<long> { 1, 2, 3 },
                    TestSamples.exampleMovie.Trailer,
                    TestSamples.exampleMovie.Bio,
                    TestSamples.exampleMovie.MainImageName,
                    TestSamples.allowedRoles);

                var addedReview = sut.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleReview.MovieId,
                   5,
                   TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                var addedReview2 = sut.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleReview.MovieId,
                   4,
                   TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                var addedReview3 = sut.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleReview.MovieId,
                   5,
                   TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                var updatedReview = sut.UpdateReviewAsync(
                   TestSamples.exampleReview.Id,
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleReview.MovieId,
                   3,
                   TestSamples.oldReviewRating,
                   TestSamples.exampleReview2.Text,
                    TestSamples.allowedRoles).Result;

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(assertContext, mockBusinessValidator.Object);
                var sut2 = new MovieServices(assertContext, mockBusinessValidator.Object);              

                var result = sut2.GetMovieWithBasicInfoAsync(1).Result;

                Assert.AreEqual(result.Rating, 4);
                Assert.AreEqual(result.AllRatingsSum, 12);
                Assert.AreEqual(result.TotalRatings, 3);

            }
        }

        [TestMethod]
        public void DeleteReviewAsync_Should_CorrectlyDeleteReview()
        {
            var options = TestUtils.GetOptions(nameof(DeleteReviewAsync_Should_CorrectlyDeleteReview));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);

                var addedReview = sut.AddReviewAsync(
                    TestSamples.exampleReview.ApplicationUserName,
                    TestSamples.exampleReview.ApplicationUserId,
                    TestSamples.exampleReview.MovieId,
                    TestSamples.exampleReview.Rating,
                    TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                var result = sut.DeleteReviewAsync(TestSamples.exampleReview.Id,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                Assert.AreEqual(assertContext.Reviews.Count(), 0);                
            }
        }

        [TestMethod]
        public void DeleteReviewAsync_Should_ReturnCorrectReview()
        {
            var options = TestUtils.GetOptions(nameof(DeleteReviewAsync_Should_ReturnCorrectReview));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(arrangeContext, mockBusinessValidator.Object);              

                var addedReview = sut.AddReviewAsync(
                   TestSamples.exampleReview.ApplicationUserName,
                   TestSamples.exampleReview.ApplicationUserId,
                   TestSamples.exampleReview.MovieId,
                   TestSamples.exampleReview.Rating,
                   TestSamples.exampleReview.Text,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ReviewServices(assertContext, mockBusinessValidator.Object);

                var result = sut.DeleteReviewAsync(TestSamples.exampleReview.Id,
                    TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(result, typeof(Review));
                Assert.AreEqual(result.Id, TestSamples.exampleReview.Id);
                Assert.AreEqual(result.MovieId, TestSamples.exampleReview.MovieId);
                Assert.AreEqual(result.Rating, TestSamples.exampleReview.Rating);
                Assert.AreEqual(result.Text, TestSamples.exampleReview.Text);
                Assert.AreEqual(result.ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
                Assert.AreEqual(result.ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
            }
        }
    }
}
