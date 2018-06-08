using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Controllers;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.ViewModels.Home;
using BookwormTests.MockData;
using FakeItEasy;
using NUnit.Framework;

namespace BookwormTests.Unit_Tests.Controllers
{
    [TestFixture]
    public class LoginControllerUnitTests
    {
        private LoginController _loginController;
        private ILoginService _fakeLoginService;
        private IRepository<User> _fakeRepository;
        private MockUsers _mockUserData;

        [SetUp]
        public void SetUp()
        {
            _fakeLoginService = A.Fake<ILoginService>();
            var _fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _loginController = new LoginController(_fakeLoginService);
            _fakeRepository = new Repository<User>(_fakeBookwormDbContext);
            _mockUserData = new MockUsers();
        }


        [Test]
        public void When_LoginPageCalled_ReturnLoginView()
        {
            // Act
            ViewResult result = _loginController.LoginPage() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Login Page"));
        }

        [Test]
        public void GivenCorrectLoginDetails_WhenAttemptLogin_ReturnUsersProfile()
        {
            // Arrange
            var fakeLoginDetails = A.Fake<UserLoginViewModel>();
            fakeLoginDetails.Password = "bill buck";
            fakeLoginDetails.Email = "abc@gmail.com";

            var fakeUser = _mockUserData.FakeUser1;

            A.CallTo(() => _fakeLoginService.GetUserDetails(fakeLoginDetails)).Returns(fakeUser);

            var fakeHttpContext = A.Fake<HttpContextBase>();
            var session = A.Fake<HttpSessionStateBase>();
            session["userId"] = 7;
            A.CallTo(() => fakeHttpContext.Session).Returns(session);
            ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                _loginController);

            _loginController.ControllerContext = context;


            // Act
            var result = _loginController.AttemptLogin(fakeLoginDetails) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Contains("Profiles"));
            Assert.IsTrue(values.Contains("MyProfile"));
        }

        [Test]
        public void GivenIncorrectLogin_WhenAttemptLogin_ReturnErrorMessageInUserLoginViewModel()
        {
            // Arrange
            var fakeLoginDetails = A.Fake<UserLoginViewModel>();
            fakeLoginDetails.Password = "mypassword";
            fakeLoginDetails.Email = "abc@gmail.com";

            A.CallTo(() => _fakeLoginService.GetUserDetails(fakeLoginDetails)).Returns(null);

            // Act
            ViewResult result = _loginController.AttemptLogin(fakeLoginDetails) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOf<UserLoginViewModel>(result.Model);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Error Occured"));
        }
    }
}