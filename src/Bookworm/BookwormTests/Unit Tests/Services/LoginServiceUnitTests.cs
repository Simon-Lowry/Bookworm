using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using Bookworm.ViewModels.Home;
using NUnit.Framework;
using FakeItEasy;
using BookwormTests.MockData;

namespace BookwormTests.Unit_Tests.Services
{
    [TestFixture]
    public class LoginServiceUnitTest
    {
        private IRepository<User> _userRepository;
        private MockUsers _mockUserData;
        private ILoginService _loginService;


        [SetUp]
        public void SetUp()
        {
            var fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _userRepository = A.Fake<Repository<User>>();
            _mockUserData = new MockUsers();
            _loginService = new LoginService(_userRepository);
        }


        [Test]
        public void GivenValidLoginDetails_WhenCheckLoginDetailsCalled_ReturnUser()
        {
            // Arrange
            var loginDetails = A.Fake<UserLoginViewModel>();
            loginDetails.Email = "simon.lowry@gmail.com";
            loginDetails.Password = "CantGuessThis";

            List<User> users = A.Fake<List<User>>();
            var fakeUserDetails = A.Fake<User>();
            fakeUserDetails.Email = "simon.lowry@gmail.com";
            fakeUserDetails.Password = "CantGuessThis";
            users.Add(fakeUserDetails);

            A.CallTo(() => _userRepository.GetListOf()).Returns(users);

            // Act
            var result = _loginService.GetUserDetails(loginDetails);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.AreSame(result, fakeUserDetails);
            A.CallTo(() => _userRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenInvalidLoginDetails_WhenGetLoginDetailsCalled_ReturnNull()
        {
            // Arrange
            var loginDetails = A.Fake<UserLoginViewModel>();
            loginDetails.Email = "bakc@gmail.com";
            loginDetails.Password = "CantGuessThis";

            List<User> users = A.Fake<List<User>>();
            var fakeUserDetails = A.Fake<User>();
            fakeUserDetails.Email = "simon.lowry@gmail.com";
            fakeUserDetails.Password = "CantGuessThis";
            users.Add(fakeUserDetails);

            A.CallTo(() => _userRepository.GetListOf()).Returns(users);

            // Act
            var result = _loginService.GetUserDetails(loginDetails);

            // Assert
            Assert.That(result, Is.Null);
            Assert.AreNotSame(result, fakeUserDetails);
            A.CallTo(() => _userRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}