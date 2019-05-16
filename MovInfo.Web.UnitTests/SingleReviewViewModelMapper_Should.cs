using Castle.Core.Configuration;
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
    public class SingleReviewViewModelMapper_Should
    {
        [TestMethod]
        public void MapFrom_Should_CorrectlyMapReviewToSingleReviewViewModel()
        {
            //Arrange         
            var sut = new SingleReviewViewModelMapper();

            //Act
            var result = sut.MapFrom(TestSamples.exampleReview);

            //Assert
            Assert.IsInstanceOfType(result, typeof(SingleReviewViewModel));
            Assert.AreEqual(result.Id, TestSamples.exampleReview.Id);
            Assert.AreEqual(result.MovieId, TestSamples.exampleReview.MovieId);
            Assert.AreEqual(result.Rating, TestSamples.exampleReview.Rating);
            Assert.AreEqual(result.Text, TestSamples.exampleReview.Text);
            Assert.AreEqual(result.ApplicationUserName, TestSamples.exampleReview.ApplicationUserName);
            Assert.AreEqual(result.ApplicationUserId, TestSamples.exampleReview.ApplicationUserId);
        }

    }
}
