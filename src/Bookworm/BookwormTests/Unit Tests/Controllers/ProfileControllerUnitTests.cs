using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Controllers;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using FakeItEasy;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web.Routing;
using Bookworm.ViewModels.Profiles;
using BookwormTests.MockData;

namespace BookwormTests.Unit_Tests.Controllers
{
    [TestFixture]
    public class ProfileControllerUnitTests
    {
        private ProfilesController _profilesController;
        private IProfileService _fakeProfileService;
        private IBookService _fakeBookService;
        private IRepository<User> _fakeRepository;
        private MockUsers _mockUserData;
        private String controllerName = "Profiles";
        private String actionCalledOtherUsersProfile = "OtherUsersProfile";

        [SetUp]
        public void SetUp()
        {
            _fakeProfileService = A.Fake<IProfileService>();
            _fakeBookService = A.Fake<IBookService>();
            var fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _profilesController = new ProfilesController(_fakeProfileService, _fakeBookService);
            _fakeRepository = new Repository<User>(fakeBookwormDbContext);
            _mockUserData = new MockUsers();
        }

        [Test]
        public void When_LogoutCalled_ReturnToLoginPage()
        {
            // Act
            RedirectToRouteResult result = _profilesController.Logout() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Contains("Login"));
            Assert.IsTrue(values.Contains("LoginPage"));
        }

        [Test]
        public void When_OtherUsersProfileCalled_ReturnOtherUsersProfileView()
        {
            // Arrange
            var combinedUsersDetails = A.Fake<CombinedUserProfilesViewModel>();

            var fakeHttpContext = A.Fake<HttpContextBase>();
            var session = A.Fake<HttpSessionStateBase>();
            session["userId"] = 7;
            A.CallTo(() => fakeHttpContext.Session).Returns(session);
            ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                _profilesController);

            _profilesController.ControllerContext = context;

            // Act
            ViewResult result = _profilesController.OtherUsersProfile(10) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Other User's Profile"));
        }


        [Test]
        public void GivenUserDetailsWhen_MyProfileCalled_ReturnProfileView()
        {
            // Arrange
            var fakeUser = A.Fake<User>();
            fakeUser.FirstName = "Bill";
            fakeUser.LastName = "Boyd";
            fakeUser.Email = "abc@mail.com";
            fakeUser.Password = "bill buck";

            var fakeHttpContext = A.Fake<HttpContextBase>();
            var session = A.Fake<HttpSessionStateBase>();
            session["userId"] = 7;
            A.CallTo(() => fakeHttpContext.Session).Returns(session);
            ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                _profilesController);
            _profilesController.ControllerContext = context;

            // Act
            var result = _profilesController.MyProfile() as ViewResult;
        
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOf<MyProfileViewModel>(result.Model);
            Assert.That(result.ViewBag.Message, Is.EqualTo("My Profile"));
        }


        [Test]
        public void GivenUsersAreConnected_WhenOtherUsersProfileRequested_ReturnCombinedProfilesInPublicMode()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = 10;

            var fakeOtherUser = _mockUserData.FakeUser2;

            var fakeCombinedUserDetails = A.Fake<CombinedUserProfilesViewModel>();
            fakeCombinedUserDetails.MyUserDetails = _mockUserData.FakeUser1;
            fakeCombinedUserDetails.OtherUserDetails = fakeOtherUser;
            fakeCombinedUserDetails.UsersAreConnected = true;

            var fakeHttpContext = A.Fake<HttpContextBase>();
            var session = A.Fake<HttpSessionStateBase>();
            session["userId"] = 7;
            A.CallTo(() => fakeHttpContext.Session).Returns(session);
            ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                _profilesController);
            _profilesController.ControllerContext = context;

            A.CallTo(() => _fakeProfileService.GetUserDetails(fakeConnection.UserId)).Returns(_mockUserData.FakeUser1);
            A.CallTo(() => _fakeProfileService.AreUsersConnected(fakeConnection)).Returns(true);

            // Act
            var result = _profilesController.OtherUsersProfile(10)  as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOf<CombinedUserProfilesViewModel>(result.Model);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Other User's Profile"));
        }


        [Test]
        public void GivenUsersAreNotConnected_WhenOtherUsersProfileRequested_ReturnCombinedProfilesInPrivateMode()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = 10;

            var fakeOtherUser = _mockUserData.FakeUser2;

            var fakeCombinedUserDetails = A.Fake<CombinedUserProfilesViewModel>();
            fakeCombinedUserDetails.MyUserDetails = _mockUserData.FakeUser1;
            fakeCombinedUserDetails.OtherUserDetails = fakeOtherUser;
            fakeCombinedUserDetails.UsersAreConnected = false;

            var fakeHttpContext = A.Fake<HttpContextBase>();
            var session = A.Fake<HttpSessionStateBase>();
            session["userId"] = 7;
            A.CallTo(() => fakeHttpContext.Session).Returns(session);
            ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                _profilesController);
            _profilesController.ControllerContext = context;

            A.CallTo(() => _fakeProfileService.GetUserDetails(fakeConnection.UserId)).Returns(_mockUserData.FakeUser1);
            A.CallTo(() => _fakeProfileService.AreUsersConnected(fakeConnection)).Returns(false);

            // Act - needs testing
            var result = _profilesController.OtherUsersProfile(10) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOf<CombinedUserProfilesViewModel>(result.Model);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Other User's Profile"));
        }


        [Test]
        public void GivenValidConnectionDetails_WhenCreateConnectionCalled_ReturnCorrectCombinedProfileViewModel()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = 10;

            var fakeCombinedUserDetails = A.Fake<CombinedUserProfilesViewModel>();
            fakeCombinedUserDetails.Connection = fakeConnection;
            fakeCombinedUserDetails.MyUserDetails = _mockUserData.FakeUser1;

            A.CallTo(() => _fakeProfileService.AddConnection(fakeConnection)).Returns(true);

            // Act
            var result = _profilesController.CreateConnectionBetweenUsers(fakeCombinedUserDetails) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var userDetails = values.First();

            Assert.AreSame(userDetails, fakeCombinedUserDetails.MyUserDetails);
            Assert.IsTrue(values.Contains(CombinedUserProfilesViewModel.Success));
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledOtherUsersProfile));
            A.CallTo(() => _fakeProfileService.AddConnection(fakeConnection)).MustHaveHappened(Repeated.Exactly.Once);
        }


        // UI needs to be added so we can have the viewbag to assertain correct view was called based on failure or success
        [Test]
        public void GivenInvalidConnectionDetails_WhenCreateConnectionCalled_ReturnCorrectCombinedProfileViewModel()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = 10;

            var fakeCombinedUserDetails = A.Fake<CombinedUserProfilesViewModel>();
            fakeCombinedUserDetails.MyUserDetails = _mockUserData.FakeUser1;

            A.CallTo(() => _fakeProfileService.AddConnection(fakeConnection)).Returns(false);

            // Act
            var result = _profilesController.CreateConnectionBetweenUsers( fakeCombinedUserDetails) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var userDetails = values.First();

            Assert.AreSame(userDetails, fakeCombinedUserDetails.MyUserDetails);
            Assert.IsTrue(values.Contains(CombinedUserProfilesViewModel.Failed));
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledOtherUsersProfile));
        }


        [Test]
        public void GivenValidConnectionDetails_WhenDeleteConnectionCalled_ReturnCorrectCombinedProfileViewModel()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = 10;

            var fakeCombinedUserDetails = A.Fake<CombinedUserProfilesViewModel>();
            fakeCombinedUserDetails.MyUserDetails = _mockUserData.FakeUser1;

            A.CallTo(() => _fakeProfileService.DeleteConnection(fakeConnection)).Returns(true);

            // Act
            var result = _profilesController.DeleteConnectionBetweenUsers(fakeCombinedUserDetails) as RedirectToRouteResult;


            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var userDetails = values.First();

            Assert.AreSame(userDetails, fakeCombinedUserDetails.MyUserDetails);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledOtherUsersProfile));
        }

        [Test]
        public void GivenInvalidConnectionDetails_WhenDeleteConnectionCalled_ReturnCorrectCombinedProfileViewModel()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = -1;

            var fakeCombinedUserDetails = A.Fake<CombinedUserProfilesViewModel>();
            fakeCombinedUserDetails.MyUserDetails = _mockUserData.FakeUser1;

            A.CallTo(() => _fakeProfileService.DeleteConnection(fakeConnection)).Returns(false);

            // Act
            var result = _profilesController.DeleteConnectionBetweenUsers(fakeCombinedUserDetails) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var userDetails = values.First();

            Assert.AreSame(userDetails, fakeCombinedUserDetails.MyUserDetails);
            Assert.IsTrue(values.Contains(CombinedUserProfilesViewModel.Failed));
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledOtherUsersProfile));
        }
    }
}