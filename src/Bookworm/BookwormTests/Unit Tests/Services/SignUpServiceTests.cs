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
    public class SignUpServiceTests
    {
        private IRepository<User> _userRepository;
        private MockUsers _mockUserData;
        private ISignUpService _signUpService;


        [SetUp]
        public void SetUp()
        {
            var fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _userRepository = A.Fake<Repository<User>>();
            _mockUserData = new MockUsers();
            _signUpService = new SignUpService(_userRepository);
        }


        [Test]
        public void GivenUserDetails_WhenAddUserCalled_ReturnTrue()
        {
            // Arrange
            var fakeSignUpViewModel = A.Fake<UserSignUpViewModel>();
            fakeSignUpViewModel.LastName = _mockUserData.FakeUser1.LastName;
            fakeSignUpViewModel.FirstName = _mockUserData.FakeUser1.FirstName;
            fakeSignUpViewModel.Country = _mockUserData.FakeUser1.Country;
            fakeSignUpViewModel.City = _mockUserData.FakeUser1.City;
            fakeSignUpViewModel.Password = _mockUserData.FakeUser1.Password;
            fakeSignUpViewModel.Email = _mockUserData.FakeUser1.Email;
            fakeSignUpViewModel.DOB = "1990-10-10 00:00:00.000";
            
            var fakeUser = A.Fake<User>();
 
            A.CallTo(() => _userRepository.Create(A<User>._)).WithAnyArguments().Returns(true);

            // Act
            var result = _signUpService.AddUser(fakeSignUpViewModel);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(true));
            A.CallTo(() => _userRepository.Create(A<User>._)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidLoginDetails_WhenCheckLoginDetailsCalled_ReturnUser()
        {
            // Arrange
            var signUpDetails = A.Fake<UserSignUpViewModel>();
            signUpDetails.Email = "simon.lowry@gmail.com";
            signUpDetails.Password = "CantGuessThis";

            List<User> users = A.Fake<List<User>>();
            var fakeUserDetails = A.Fake<User>();
            fakeUserDetails.Email = "simon.lowry@gmail.com";
            fakeUserDetails.Password = "CantGuessThis";
            users.Add(fakeUserDetails);

            A.CallTo(() => _userRepository.GetAll()).Returns(users);

            // Act
            var result = _signUpService.GetUserDetails(signUpDetails);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.AreSame(result, fakeUserDetails);
            A.CallTo(() => _userRepository.GetAll()).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenInvalidLoginDetails_WhenGetLoginDetailsCalled_ReturnNull()
        {
            // Arrange
            var loginDetails = A.Fake<UserSignUpViewModel>();
            loginDetails.Email = "bakc@gmail.com";
            loginDetails.Password = "CantGuessThis";

            List<User> users = A.Fake<List<User>>();
            var fakeUserDetails = A.Fake<User>();
            fakeUserDetails.Email = "simon.lowry@gmail.com";
            fakeUserDetails.Password = "CantGuessThis";
            users.Add(fakeUserDetails);

            A.CallTo(() => _userRepository.GetListOf()).Returns(users);

            // Act
            var result = _signUpService.GetUserDetails(loginDetails);

            // Assert
            Assert.That(result, Is.Null);
            Assert.AreNotSame(result, fakeUserDetails);
        }
    }
}