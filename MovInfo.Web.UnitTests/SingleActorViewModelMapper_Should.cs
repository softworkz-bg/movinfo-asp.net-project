using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Web.UnitTests
{
    [TestClass]
    public class SingleActorViewModelMapper_Should
    {
        [TestMethod]
        public void MapFrom_Should_CorrectlyMapActorToSingleActorViewModel()
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

            var sut = new SingleActorViewModelMapper(mockConfig.Object);

            //Act
            var result = sut.MapFrom(TestSamples.exampleActor);

            //Assert
            Assert.IsInstanceOfType(result, typeof(SingleActorViewModel));
            Assert.AreEqual(result.Id, TestSamples.exampleActor.Id);
            Assert.AreEqual(result.FirstName, TestSamples.exampleActor.FirstName);
            Assert.AreEqual(result.LastName, TestSamples.exampleActor.LastName);
            Assert.AreEqual(result.Bio, TestSamples.exampleActor.Bio);
            Assert.AreEqual(result.MainImageName, TestSamples.exampleActor.ProfileImageName);
            Assert.AreEqual(result.FullImagePath, TestSamples.exampleActor.ProfileImageName);
        }
    }
}
