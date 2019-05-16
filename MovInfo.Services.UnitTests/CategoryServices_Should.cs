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
    public class CatgoryServices_Should
    {
        [TestMethod]
        public void FindCategoriesAsync_Should_ReturnCorrectCategories()
        {
            var options = TestUtils.GetOptions(nameof(FindCategoriesAsync_Should_ReturnCorrectCategories));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Categories.Add(TestSamples.exampleCategory);
                arrangeContext.Categories.Add(TestSamples.exampleCategory2);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(assertContext, mockBusinessValidator.Object);

                var result = sut.FindCategoriesAsync("ExampleTitle", "2");

                Assert.IsInstanceOfType(result.Result.First(), typeof(Category));
                Assert.AreEqual(result.Result.First().Title, "ExampleTitle");
                Assert.AreEqual(result.Result.Count, 1);
            }
        }

        [TestMethod]
        public void GetCategoryMoviesCorrectly()
        {
            var options = TestUtils.GetOptions(nameof(GetCategoryMoviesCorrectly));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Categories.Add(TestSamples.exampleCategory);
                arrangeContext.Movies.Add(TestSamples.exampleMovie);

                TestSamples.exampleCategory.MovieCategories.Add(TestSamples.exampleMoviesCategories);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();

                var sut = new CategoryServices(assertContext, mockBusinessValidator.Object);

                var result = sut.GetCategoryMoviesAsync(1).Result;

                Assert.IsInstanceOfType(result.First(), typeof(Movie));
                Assert.AreEqual(result.First().Id, 1);
            }
        }

        [TestMethod]
        public void ShowActorInfoCorrectly()
        {
            var options = TestUtils.GetOptions(nameof(ShowActorInfoCorrectly));

            using (var arrangeContext = new MovInfoContext(options))
            {
                arrangeContext.Categories.Add(TestSamples.exampleCategory);
                arrangeContext.Movies.Add(TestSamples.exampleMovie);

                TestSamples.exampleCategory.MovieCategories.Add(TestSamples.exampleMoviesCategories);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();

                var sut = new CategoryServices(assertContext, mockBusinessValidator.Object);

                var result = sut.ShowCategoryInfoAsync(1);

                Assert.AreEqual(assertContext.Categories.FirstOrDefault().Title, TestSamples.exampleCategory.Title);
            }
        }

        [TestMethod]
        public void EditCategory_Correctly()
        {
            var options = TestUtils.GetOptions(nameof(EditCategory_Correctly));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(arrangeContext, mockBusinessValidator.Object);

                var addedCategory = sut.AddCategoryAsync(
                    TestSamples.exampleCategory.Title,
                    TestSamples.allowedRoles);

                var editedCategory = sut.EditCategoryAsync(
                    TestSamples.exampleCategory.Id,
                    TestSamples.exampleCategory.Title,
                    new List<long> { 1, 2 },
                    TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Categories.First().Id, TestSamples.exampleCategory.Id);
                Assert.AreEqual(assertContext.Categories.First().Title, TestSamples.exampleCategory.Title);
                Assert.AreEqual(assertContext.Categories.Include(x => x.MovieCategories).First().MovieCategories.Count, 2);
            }
        }

        [TestMethod]
        public void ReturnCorrectlyEdited_Category()
        {
            var options = TestUtils.GetOptions(nameof(ReturnCorrectlyEdited_Category));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(arrangeContext, mockBusinessValidator.Object);

                var addedCategory = sut.AddCategoryAsync(
                   TestSamples.exampleCategory.Title,
                   TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(assertContext, mockBusinessValidator.Object);

                var editedCategory = sut.EditCategoryAsync(
                     TestSamples.exampleCategory.Id,
                    TestSamples.exampleCategory.Title,
                    new List<long> { 1, 2 },
                    TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(editedCategory, typeof(Category));
                Assert.AreEqual(editedCategory.Id, TestSamples.exampleCategory.Id);
                Assert.AreEqual(editedCategory.Title, TestSamples.exampleCategory.Title);
                Assert.AreEqual(editedCategory.MovieCategories.Count, 2);
            }
        }

        [TestMethod]
        public void DeleteCategoryCorrectly()
        {
            var options = TestUtils.GetOptions(nameof(DeleteCategoryCorrectly));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(arrangeContext, mockBusinessValidator.Object);

                var addedCategory = sut.AddCategoryAsync(TestSamples.exampleCategory.Title, TestSamples.allowedRoles);

                var deletedCategory = sut.DeleteCategoryAsync(TestSamples.exampleCategory.Id, TestSamples.allowedRoles).Result;

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                Assert.AreEqual(assertContext.Categories.Count(), 0);
            }
        }

        [TestMethod]
        public void CorrectlyReturnDeleted_Category()
        {
            var options = TestUtils.GetOptions(nameof(CorrectlyReturnDeleted_Category));

            using (var arrangeContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(arrangeContext, mockBusinessValidator.Object);

                var addedCategory = sut.AddCategoryAsync(TestSamples.exampleCategory.Title, TestSamples.allowedRoles);

                arrangeContext.SaveChanges();
            }

            using (var assertContext = new MovInfoContext(options))
            {
                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
                var sut = new CategoryServices(assertContext, mockBusinessValidator.Object);

                var deletedCategory = sut.DeleteCategoryAsync(
                    TestSamples.exampleCategory.Id, TestSamples.allowedRoles).Result;

                Assert.IsInstanceOfType(deletedCategory, typeof(Category));
                Assert.AreEqual(deletedCategory.Id, TestSamples.exampleCategory.Id);
                Assert.AreEqual(deletedCategory.Title, TestSamples.exampleCategory.Title);
            }
        }
    }
}
