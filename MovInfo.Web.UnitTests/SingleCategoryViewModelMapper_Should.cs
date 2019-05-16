using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Web.UnitTests
{
    [TestClass]
    public class SingleCategoryViewModelMapper_Should
    {
        [TestMethod]
        public void MapFrom_Should_CorrectlyMapCategoryToSingleCategoryViewModel()
        {
            //Arrange         
            var sut = new SingleCategoryViewModelMapper();

            //Act
            var result = sut.MapFrom(TestSamples.exampleCategory);

            //Assert
            Assert.IsInstanceOfType(result, typeof(SingleCategoryViewModel));
            Assert.AreEqual(result.Id, TestSamples.exampleCategory.Id);
            Assert.AreEqual(result.Title, TestSamples.exampleCategory.Title);        }
    }
}
