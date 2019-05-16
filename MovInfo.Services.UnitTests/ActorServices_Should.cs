using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MovInfo.Services.UnitTests
{
    [TestClass]
    public class ActorServices_Should
    {
        [TestMethod]
        public void FindActorsAsync_Should_ReturnCorrectActors()
        {
            var options = TestUtils.GetOptions(nameof(FindActorsAsync_Should_ReturnCorrectActors));

            using (var arrangeContext = new MovInfoContext(options))
            {               
                arrangeContext.Actors.Add(TestSamples.exampleActor);
                arrangeContext.Actors.Add(TestSamples.exampleActor2);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(assertContext, mockBusinessValidator.Object);

                var result = sut.FindActorsAsync("ExampleFirstName", "2");
                Assert.AreEqual(result.Result.First().FirstName, "ExampleFirstName");
                Assert.AreEqual(result.Result.Count, 1);
            }
        }

        [TestMethod]
        public void GetActorMoviesCorrectly_ShouldReturnListOfMovies()
        {
            var options = TestUtils.GetOptions(nameof(GetActorMoviesCorrectly_ShouldReturnListOfMovies));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Actors.Add(TestSamples.exampleActor);
                arrangeContext.Movies.Add(TestSamples.exampleMovie);

                TestSamples.exampleActor.MoviesActors.Add(TestSamples.exampleMoviesActors);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();

                var sut = new ActorServices(assertContext, mockBusinessValidator.Object);

                var result = sut.GetActorMoviesAsync(1).Result;

                Assert.IsInstanceOfType(result[0], typeof(Movie));
                Assert.AreEqual(result.First().Id, 1);
            }
        }

        [TestMethod]
        public void AddActor_Should_Succeed()
        {
            var options = TestUtils.GetOptions(nameof(AddActor_Should_Succeed));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Actors.Add(TestSamples.exampleActor);
                arrangeContext.Movies.Add(TestSamples.exampleMovie);

                TestSamples.exampleActor.MoviesActors.Add(TestSamples.exampleMoviesActors);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(assertContext, mockBusinessValidator.Object);
                Assert.AreEqual(assertContext.Actors.Count(), 1);
            }
        }

        [TestMethod]
        public void AddActor_Should_AddCorrect_Actor()
        {
            var options = TestUtils.GetOptions(nameof(AddActor_Should_AddCorrect_Actor));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Actors.Add(TestSamples.exampleActor);
                arrangeContext.Movies.Add(TestSamples.exampleMovie);

                TestSamples.exampleActor.MoviesActors.Add(TestSamples.exampleMoviesActors);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.IsInstanceOfType(assertContext.Actors.FirstOrDefault(), typeof(Actor));
                Assert.AreEqual(assertContext.Actors.FirstOrDefault().FirstName, TestSamples.exampleActor.FirstName);
                Assert.AreEqual(assertContext.Actors.FirstOrDefault().LastName, TestSamples.exampleActor.LastName);
                Assert.AreEqual(assertContext.Actors.FirstOrDefault().Bio, TestSamples.exampleActor.Bio);
                Assert.AreEqual(assertContext.Actors.FirstOrDefault().ProfileImageName, TestSamples.exampleActor.ProfileImageName);
                Assert.AreEqual(assertContext.MoviesActors.FirstOrDefault().MovieId, 1);
            }
        }

        [TestMethod]
        public void ShowActorInfoCorrectly()
        {
            var options = TestUtils.GetOptions(nameof(ShowActorInfoCorrectly));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Actors.Add(TestSamples.exampleActor);
                arrangeContext.Movies.Add(TestSamples.exampleMovie);

                TestSamples.exampleActor.MoviesActors.Add(TestSamples.exampleMoviesActors);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();

                var sut = new ActorServices(assertContext, mockBusinessValidator.Object);

                var result = sut.ShowActorInfoAsync(1);

                Assert.AreEqual(assertContext.Actors.FirstOrDefault().FirstName, TestSamples.exampleActor.FirstName);
                Assert.AreEqual(assertContext.Actors.FirstOrDefault().LastName, TestSamples.exampleActor.LastName);
                Assert.AreEqual(assertContext.Actors.FirstOrDefault().Bio, TestSamples.exampleActor.Bio);
            }
        }

        [TestMethod]
        public void EditActor_Should_CorrectlyEdit_Actor()
        {
            var options = TestUtils.GetOptions(nameof(EditActor_Should_CorrectlyEdit_Actor));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(arrangeContext, mockBusinessValidator.Object);

                var addedActor = sut.AddActorAsync(
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                var editedActor = sut.EditActorAsync(
                    TestSamples.exampleActor.Id,
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Actors.First().Id, TestSamples.exampleActor.Id);
                Assert.AreEqual(assertContext.Actors.First().FirstName, TestSamples.exampleActor2.FirstName);
                Assert.AreEqual(assertContext.Actors.First().LastName, TestSamples.exampleActor2.LastName);
                Assert.AreEqual(assertContext.Actors.First().Bio, TestSamples.exampleActor2.Bio);
                Assert.AreEqual(assertContext.Actors.First().ProfileImageName, TestSamples.exampleActor2.ProfileImageName);
            }
        }

        [TestMethod]
        public void EditActor_Should_ReturnCorrectlyEdited_Actor()
        {
            var options = TestUtils.GetOptions(nameof(EditActor_Should_ReturnCorrectlyEdited_Actor));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(arrangeContext, mockBusinessValidator.Object);

                var addedActor = sut.AddActorAsync(
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(assertContext, mockBusinessValidator.Object);

                var editedActor = sut.EditActorAsync(
                    TestSamples.exampleActor.Id,
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(editedActor, typeof(Actor));
                Assert.AreEqual(editedActor.Id, TestSamples.exampleActor.Id);
                Assert.AreEqual(editedActor.FirstName, TestSamples.exampleActor2.FirstName);
                Assert.AreEqual(editedActor.LastName, TestSamples.exampleActor2.LastName);
                Assert.AreEqual(editedActor.Bio, TestSamples.exampleActor2.Bio);
                Assert.AreEqual(editedActor.ProfileImageName, TestSamples.exampleActor2.ProfileImageName);            }
        }

        [TestMethod]
        public void DeleteActorCorrectly()
        {
            var options = TestUtils.GetOptions(nameof(DeleteActorCorrectly));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(arrangeContext, mockBusinessValidator.Object);

                var addedActor = sut.AddActorAsync(
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                var deletedActor = sut.DeleteActorAsync(
                    TestSamples.exampleActor.Id, TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Actors.Count(), 0);
            }
        }

        [TestMethod]
        public void CorrectlyReturnDeleted_Actor()
        {
            var options = TestUtils.GetOptions(nameof(CorrectlyReturnDeleted_Actor));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(arrangeContext, mockBusinessValidator.Object);

                var addedActor = sut.AddActorAsync(
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new ActorServices(assertContext, mockBusinessValidator.Object);

                var deletedActor = sut.DeleteActorAsync(
                    TestSamples.exampleActor.Id, TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(deletedActor, typeof(Actor));
                Assert.AreEqual(deletedActor.Id, TestSamples.exampleActor.Id);
                Assert.AreEqual(deletedActor.FirstName, TestSamples.exampleActor.FirstName);
                Assert.AreEqual(deletedActor.LastName, TestSamples.exampleActor.LastName);
                Assert.AreEqual(deletedActor.Bio, TestSamples.exampleActor.Bio);
                Assert.AreEqual(deletedActor.ProfileImageName, TestSamples.exampleActor.ProfileImageName);
            }
        }
    }
}