using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovInfo.Models;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Web.UnitTests
{
    [TestClass]
    public class SingleMovieViewModelMapper_Should
    {
        [TestMethod]
        public void MapFrom_Should_CorrectlyMapMovieToSingleMovieViewModel()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            var oneSectionMock = new Mock<IConfigurationSection>();
            oneSectionMock.Setup(s => s.Value).Returns("1");
            var twoSectionMock = new Mock<IConfigurationSection>();
            twoSectionMock.Setup(s => s.Value).Returns("2");
            var fooBarSectionMock = new Mock<IConfigurationSection>();
            fooBarSectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { oneSectionMock.Object, twoSectionMock.Object });
            mockConfig.Setup(c => c.GetSection("DefaultImageFolder")).Returns(fooBarSectionMock.Object);

            var sut = new SingleMovieViewModelMapper(mockConfig.Object);

            //Act
            var result = sut.MapFrom(TestSamples.exampleMovie);

            //Assert
            Assert.IsInstanceOfType(result, typeof(SingleMovieViewModel));
            Assert.AreEqual(result.Id, TestSamples.exampleMovie.Id);
            Assert.AreEqual(result.Name, TestSamples.exampleMovie.Name);
            Assert.AreEqual(result.DateCreated, TestSamples.exampleMovie.DateCreated);
            Assert.AreEqual(result.Trailer, TestSamples.exampleMovie.Trailer);
            Assert.AreEqual(result.Bio, TestSamples.exampleMovie.Bio);
            Assert.AreEqual(result.NumberOfRatings, TestSamples.exampleMovie.TotalRatings);
            Assert.AreEqual(result.MainImageName, TestSamples.exampleMovie.MainImageName);
            Assert.AreEqual(result.FullImagePath, TestSamples.exampleMovie.MainImageName);
        }
            
    }
}
