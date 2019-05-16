//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Internal;
//using Microsoft.Extensions.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using MovInfo.Data;
//using MovInfo.ImageOptimizer;
//using MovInfo.Models;
//using MovInfo.Services;
//using MovInfo.Services.Contracts;
//using MovInfo.Web.Controllers;
//using MovInfo.Web.Mappers;
//using MovInfo.Web.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace MovInfo.Web.UnitTests
//{
//    [TestClass]
//    public class MovieController_Should
//    {
//        [TestMethod]
//        public void SaveSingleMovie_Should_Succeed()
//        {
//            var options = TestUtils.GetOptions(nameof(SaveSingleMovie_Should_Succeed));

//            using (var arrangeContext = new MovInfoContext(options))
//            {
//                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
//                var mockMovieService = new Mock<IMovieServices>();
//                var mockActorService = new Mock<IActorServices>();
//                var mockCategoryService = new Mock<ICategoryServices>();
//                var mockImageOptimizer = new Mock<IMovInfoImageOptimizer>();

//                var mockActorMapper = new Mock<IViewModelMapper<Actor, SingleActorViewModel>>();
//                var mockCategoryMapper = new Mock<IViewModelMapper<Category, SingleCategoryViewModel>>();
//                var mockReviewMapper = new Mock<IViewModelMapper<Review, SingleReviewViewModel>>();
//                var mockMovieMapper = new Mock<IViewModelMapper<Movie, SingleMovieViewModel>>();
//                var mockConfig = new Mock<IConfiguration>();

//                var image = new FormFile(new MemoryStream(), 0, 10, "ExampleName", "ExampleFile");

//                var viewModel = new SingleMovieViewModel()
//                {
//                    Name = "ExampleName",
//                    DateCreated = new DateTime(2019, 12, 12),
//                    Trailer = "https://example.com",
//                    Bio = "Example Movie Bio",
//                    MainImage = image
//                };

//                var sut = new MovieController(
//                    mockMovieService.Object, 
//                    mockImageOptimizer.Object, 
//                    mockActorService.Object, 
//                    mockCategoryService.Object,
//                    mockConfig.Object,
//                    mockActorMapper.Object,
//                    mockCategoryMapper.Object,
//                    mockReviewMapper.Object,
//                    mockMovieMapper.Object);

//                var result = sut.SaveSingleMovie(viewModel);

//                arrangeContext.SaveChanges();
//            }

//            using (var assertContext = new MovInfoContext(options))
//            {
//                var mockBusinessValidator = new Mock<IBusinessLogicValidator>();
//                var sut = new MovieServices(assertContext, mockBusinessValidator.Object);
//                Assert.AreEqual(assertContext.Movies.Count(), 1);
//            }
//        }

//    }
//}
