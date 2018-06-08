using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using NUnit.Framework;
using FakeItEasy;
using BookwormTests.MockData;
using System.Web.Mvc;

namespace BookwormTests.Unit_Tests.Services
{
    [TestFixture]
    public class ProfileServiceUnitTest
    {
        private IRepository<User> _fakeUserRepository;
        private IRepository<Connection> _fakeConnectionRepository;
        private MockUsers _mockUserData;
        private IProfileService _profileService;


        [SetUp]
        public void SetUp()
        {
            var fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _fakeUserRepository = A.Fake<Repository<User>>();

            var fakeBookwormDbContext2 = A.Fake<BookwormDbContext>();
            _fakeConnectionRepository = A.Fake<Repository<Connection>>();

            _profileService = new ProfileService(_fakeUserRepository, _fakeConnectionRepository);

            _mockUserData = new MockUsers();
        }


        [Test]
        public void GivenValidConnectionDetails_WhenAddConnectionCalled_ReturnTrue()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 5;
            fakeConnection.OtherUserId = 10;

            A.CallTo(() => _fakeConnectionRepository.Create(fakeConnection)).Returns(true);
            A.CallTo(() => _fakeConnectionRepository.Create(fakeConnection)).Returns(true);
            
            var result = _profileService.AddConnection(fakeConnection);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(true));
            A.CallTo(() => _fakeConnectionRepository.Create(fakeConnection)).MustHaveHappened(Repeated.Exactly.Twice);
            
        }


        [Test]
        public void GivenInvalidConnectionDetails_WhenAddConnectionCalled_ReturnFalse()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 0;
            fakeConnection.OtherUserId = -1;

            A.CallTo(() => _fakeConnectionRepository.Create(fakeConnection)).Returns(false);
            
            var result = _profileService.AddConnection(fakeConnection);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(false));
            A.CallTo(() => _fakeConnectionRepository.Create(fakeConnection)).MustHaveHappened(Repeated.Exactly.Once);
            
        }


        [Test]
        public void GivenValidConnectionDetails_WhenDeleteConnectionCalled_ReturnTrue()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 0;
            fakeConnection.OtherUserId = -1;

            A.CallTo(() => _fakeConnectionRepository.Delete(fakeConnection)).Returns(true);

            var result = _profileService.DeleteConnection(fakeConnection);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(true));
            A.CallTo(() => _fakeConnectionRepository.Delete(fakeConnection)).MustHaveHappened(Repeated.Exactly.Twice);
        }


        [Test]
        public void GivenInvalidConnectionDetails_WhenDeleteConnectionCalled_ReturnFalse()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 0;
            fakeConnection.OtherUserId = -1;

            A.CallTo(() => _fakeConnectionRepository.Delete(fakeConnection)).Returns(false);
 
            // Act
            var result = _profileService.DeleteConnection(fakeConnection);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(false));
            A.CallTo(() => _fakeConnectionRepository.Delete(fakeConnection)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidConnectionDetails_When_AreUsersConnectedCalled_ReturnTrue()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 1;
            fakeConnection.OtherUserId = 2;

            var fakeConnection2 = A.Fake<Connection>();
            fakeConnection.UserId = 1;
            fakeConnection.OtherUserId = 2;

            var fakeConnection3 = A.Fake<Connection>();
            fakeConnection.UserId = 1;
            fakeConnection.OtherUserId = -2;


            IList<Connection> cons = A.Fake<List<Connection>>();

            cons.Add(fakeConnection2);
            cons.Add(fakeConnection3);

            A.CallTo(() => _fakeConnectionRepository.GetListOf()).Returns(cons);

            // Act
            var result = _profileService.AreUsersConnected(fakeConnection);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(false));
            A.CallTo(() => _fakeConnectionRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);

        }


        [Test]
        public void GivenInvalidConnectionDetails_When_AreUsersConnectedCalled_ReturnFalse()
        {
            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 1;
            fakeConnection.OtherUserId = 2;

            var fakeConnection2 = A.Fake<Connection>();
            fakeConnection.UserId = 90;
            fakeConnection.OtherUserId = 47;

            var fakeConnection3 = A.Fake<Connection>();
            fakeConnection.UserId = 1000;
            fakeConnection.OtherUserId = 111;


            IList<Connection> cons = A.Fake<List<Connection>>();

            cons.Add(fakeConnection2);
            cons.Add(fakeConnection3);

            A.CallTo(() => _fakeConnectionRepository.GetListOf()).Returns(cons);

            // Act
            var result = _profileService.AreUsersConnected(fakeConnection);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That(result, Is.EqualTo(false));
            A.CallTo(() => _fakeConnectionRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);

        }


        [Test]
        public void GivenValidUserId_WhenGetUserDetailsCalled_ReturnUser()
        {
            // Arrange
            int userId = 1;
            User fakeUser = _mockUserData.FakeUser1;

            A.CallTo(() => _fakeUserRepository.Get(userId)).Returns(fakeUser);

            // Act
            var result = _profileService.GetUserDetails(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(result, fakeUser);
        }

        [Test]
        public void GivenInvalidUserId_WhenGetUserDetailsCalled_ReturnNull()
        {
            int userId = 1;

            A.CallTo(() => _fakeUserRepository.Get(userId)).Returns(null);

            var result = _profileService.GetUserDetails(userId);

            // Assert
            Assert.IsNull(result);
        }

    }
}