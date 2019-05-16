using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MovInfo.Services.UnitTests
{
    [TestClass]
    public class BusinessLogicValidator_Should
    {
        [TestMethod]
        public void IsUserAuthenticated_ShouldThrowCorrectUnathorisedAccessExceptionAndMessage_WhenUserIsNotAuthorised()
        {
            //Arrange
            var mockWrapUserManager = new Mock<IWrapUserManager>();
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(false);

            var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

            //Act, Assert
            var ex = Assert.ThrowsException<UnauthorizedAccessException>(() => sut.IsUserAuthenticated(BusinessLogicValidatorMessages.NotAuthorized));
            Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NotAuthorized);
        }


        [TestMethod]
        public void IsEntityFound_ShouldThrowArgumentExceptionAndCorrectMessage_WhenEntityIsNull()
        {
            //Arrange
            var mockWrapUserManager = new Mock<IWrapUserManager>();
            var mockHttpContext = new Mock<IHttpContextAccessor>();

            var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

            //Act, Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => sut.IsEntityFound(null, BusinessLogicValidatorMessages.NoSuchMovie));
            Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NoSuchMovie);
        }

        [TestMethod]
        public void IsEntityAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenIsNotNull()
        {
            //Arrange
            var mockWrapUserManager = new Mock<IWrapUserManager>();
            var mockHttpContext = new Mock<IHttpContextAccessor>();

            var obj = new object();

            var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

            //Act, Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => sut.IsEntityAlreadyPresent(obj, BusinessLogicValidatorMessages.ActorAlreadyPresent));
            Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.ActorAlreadyPresent);
        }

        [TestMethod]
        public void AreAnyEntitiesPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoEntitiesInCollection()
        {
            //Arrange
            var mockWrapUserManager = new Mock<IWrapUserManager>();
            var mockHttpContext = new Mock<IHttpContextAccessor>();

            var entitiesList = new List<object>();

            var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

            //Act, Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => sut.AreAnyEntitiesPresent(entitiesList.AsQueryable(), BusinessLogicValidatorMessages.OneOrMoreActorsMissing));
            Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.OneOrMoreActorsMissing);
        }

        [TestMethod]
        public void DoesMovieExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchMovieInDb()
        {
            var options = TestUtils.GetOptions(nameof(DoesMovieExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchMovieInDb));

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.DoesMovieExist(assertContext, TestSamples.exampleMovie.Id));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NoSuchMovie);             
            }
        }

        [TestMethod]
        public void DoesActorExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchActorInDb()
        {
            var options = TestUtils.GetOptions(nameof(DoesActorExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchActorInDb));

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.DoesActorExist(assertContext, TestSamples.exampleActor.Id));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NoSuchActor);
            }
        }


        [TestMethod]
        public void DoesReviewExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchReviewInDb()
        {
            var options = TestUtils.GetOptions(nameof(DoesReviewExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchReviewInDb));

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.DoesReviewExist(assertContext, TestSamples.exampleReview.Id));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NoSuchReview);
            }
        }


        [TestMethod]
        public void DoesCategoryExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchCategoryInDb()
        {
            var options = TestUtils.GetOptions(nameof(DoesCategoryExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchCategoryInDb));

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.DoesCategoryExist(assertContext, TestSamples.exampleCategory.Id));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NoSuchCategory);
            }
        }

        [TestMethod]
        public void DoAllCategoriesExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchCategoriesInDb()
        {
            var options = TestUtils.GetOptions(nameof(DoesCategoryExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchCategoryInDb));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var service = new CategoryServices(arrangeContext, mockBusinessValidator.Object);

                var addedCategory = service.AddCategoryAsync(
                    TestSamples.exampleCategory.Title,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();
                var categoriesIds = new long[] { 1, 2 };

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.DoAllCategoriesExist(assertContext, categoriesIds));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.OneOrMoreCategoriesMissing);
            }
        }

        [TestMethod]
        public void DoesAllActorsExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchActorsInDb()
        {
            var options = TestUtils.GetOptions(nameof(DoesCategoryExist_ShouldThrowArgumentExceptionAndCorrectMessage_WhenNoSuchCategoryInDb));


            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var service = new ActorServices(arrangeContext, mockBusinessValidator.Object);

                var addedActor = service.AddActorAsync(
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }


            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();
                var actorsIds = new long[] { 1, 2 };

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.DoAllActorsExist(assertContext, actorsIds));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.OneOrMoreActorsMissing);
            }
        }

        [TestMethod]
        public void IsMovieAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenMovieAlreadyInDb()
        {
            var options = TestUtils.GetOptions(nameof(IsMovieAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenMovieAlreadyInDb));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var service = new MovieServices(arrangeContext, mockBusinessValidator.Object);

                var addedMovie = service.AddMovieAsync(
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
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.IsMovieAlreadyPresent(assertContext, TestSamples.exampleMovie.Name, TestSamples.exampleMovie.DateCreated.Year));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.MovieAlreadyPresent);
            }
        }


        [TestMethod]
        public void IsActorAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenActorAlreadyInDb()
        {
            var options = TestUtils.GetOptions(nameof(IsActorAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenActorAlreadyInDb));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var service = new ActorServices(arrangeContext, mockBusinessValidator.Object);

                var addedActor = service.AddActorAsync(
                    TestSamples.exampleActor.FirstName,
                    TestSamples.exampleActor.LastName,
                    TestSamples.exampleActor.Bio,
                    TestSamples.exampleActor.ProfileImageName,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.IsActorAlreadyPresent(assertContext, TestSamples.exampleActor.FirstName, TestSamples.exampleActor.LastName));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.ActorAlreadyPresent);
            }
        }

        [TestMethod]
        public void IsCategoryAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenCategoryAlreadyInDb()
        {
            var options = TestUtils.GetOptions(nameof(IsCategoryAlreadyPresent_ShouldThrowArgumentExceptionAndCorrectMessage_WhenCategoryAlreadyInDb));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var service = new CategoryServices(arrangeContext, mockBusinessValidator.Object);

                var addedCategory = service.AddCategoryAsync(
                    TestSamples.exampleCategory.Title,
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockWrapUserManager = new Mock<IWrapUserManager>();
                var mockHttpContext = new Mock<IHttpContextAccessor>();

                var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

                var ex = Assert.ThrowsException<ArgumentException>(() => sut.IsCategoryAlreadyPresent(assertContext, TestSamples.exampleCategory.Title));
                Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.CategoryAlreadyPresent);
            }
        }

        //[TestMethod]
        //public void IsUserInRoleForService_ShouldThrowCorrectUnathorisedAccessExceptionAndMessage_WhenUserIsNotInRole()
        //{
        //    //Arrange
        //    var mockWrapUserManager = new Mock<IWrapUserManager>();
        //    var mockHttpContext = new Mock<IHttpContextAccessor>();
        //    var mockAppUser = new Mock<ApplicationUser>();
        //    var allowedRoles = new string[] { "Admin", "Manager" };

        //    var claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name, "username"),
        //        new Claim(ClaimTypes.NameIdentifier, "userId"),
        //        new Claim("name", "John Doe"),
        //    };
        //    var identity = new ClaimsIdentity(claims, "TestAuthType");
        //    var claimsPrincipal = new ClaimsPrincipal(identity);

        //    mockHttpContext.SetupGet(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)).Returns(claims[0]);


        //    mockHttpContext.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).Returns(claims[0]);
        //    mockWrapUserManager.Setup(x => x.WrappedUserManager.Users.First()).Returns(mockAppUser.Object);
        //    mockWrapUserManager.Setup(x => x.WrappedUserManager.IsInRoleAsync(mockAppUser.Object, "Admin").Result).Returns(false);

        //    var sut = new BusinessLogicValidator(mockHttpContext.Object, mockWrapUserManager.Object);

        //    var ex = Assert.ThrowsException<UnauthorizedAccessException>(() => sut.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized));
        //    Assert.AreEqual(ex.Message, BusinessLogicValidatorMessages.NotAuthorized);
        //}


    }
}
