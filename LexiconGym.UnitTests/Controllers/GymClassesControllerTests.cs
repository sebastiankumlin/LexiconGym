using LexiconGym.Controllers;
using LexiconGym.Core;
using LexiconGym.Core.Models;
using LexiconGym.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiconGym.UnitTests.Controllers
{
    [TestClass]
    public class GymClassesControllerTests
    {
        private Mock<IGymClassesRepository> repository;
        private GymClassesController controller;
        private const int gymClassIdNotExists = 3;

        [TestInitialize]
        public void SetUp()
        {
            repository = new Mock<IGymClassesRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(u => u.GymClasses).Returns(repository.Object);
            controller = new GymClassesController(mockUoW.Object);
        }

        private List<GymClass> GetGymClassList()
        {
            return new List<GymClass>
            {
                new GymClass
                {
                      Id =1,
                       Name = "Spinning",
                        Description = "Beginner",
                        StartTime = DateTime.Now,
                         Duration = new TimeSpan(1,0,0)
                       
                },
                new GymClass
                {
                      Id =2,
                       Name = "CrossFit",
                        Description = "Beginner",
                        StartTime = DateTime.Now,
                        Duration = new TimeSpan(1,0,0)
                }
            };
        }

        [TestMethod]
        public async Task Index_Returns_ViewResult()
        {
            var actual = await controller.Index();

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void Index_Returns_AllGymClasses()
        {
            var expected = GetGymClassList();
            repository.Setup(g => g.GetAllAsync()).ReturnsAsync(expected);

            var viewResult = controller.Index().Result as ViewResult;
            var actual = (IEnumerable<GymClass>)viewResult.Model;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Details_GetCorrectGymClass()
        {
            var gymclassId = 1;
            var exspected = GetGymClassList()[0];
            repository.Setup(g => g.GetAsync(exspected.Id)).ReturnsAsync(exspected);

            var actual = (ViewResult)controller.Details(gymclassId).Result;

            Assert.AreEqual(exspected, actual.Model);
        }

        [TestMethod]
        public void Details_NoGymClassWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = (StatusCodeResult)controller.Details(gymClassIdNotExists).Result;
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ReturnsDefaultView_ShouldREturnNull()
        {
            var result = controller.Create() as ViewResult;
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public void Edit_ReciveNullId_ShouldReturnNotFoundStatusCode()
        {
            int notFoundStatusCode = 404;

            var result = (StatusCodeResult)controller.Edit(null).Result;

            Assert.AreEqual(notFoundStatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Edit_RedirectToIndexWhenUpdateOK()
        {
            var gymClass = GetGymClassList()[0];
            var expected = "Index";

            var result = (RedirectToActionResult)controller.Edit(gymClass.Id, gymClass).Result;
            var actual = result.ActionName;
           
            Assert.AreEqual(expected, actual);
        }
    }
}
