using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Bookworm;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Controllers;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using Bookworm.ViewModels.Home;
using Bookworm.ViewModels.Profiles;
using BookwormTests.MockData;

namespace BookwormTests.Integration_Tests.Service
{
    public class ProfileServiceIntegrationTests
    {
        private ProfileService _profilesService;
        private SignUpService _signUpService;
        private String controllerName = "Profiles";
        private String actionCalledOtherUsersProfile = "OtherUsersProfile";
        private static ContainerBuilder _builder { get; set; }
        private static Autofac.IContainer Container { get; set; }
        private static MockUsers _mockData;


        [SetUp]
        public void SetUp()
        {
            _mockData = new MockUsers();
           
            _builder = new ContainerBuilder();
            {
                _builder.RegisterType<ProfileService>().As<IProfileService>();
                _builder.RegisterControllers(typeof(MvcApplication).Assembly);

                // DB registration
                _builder.RegisterType<BookwormDbContext>().AsSelf().As<IBookwormDbContext>();

                // Unit of Work registration            
                _builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

                // Services registration
                _builder.RegisterType<SignUpService>().As<ISignUpService>();
                _builder.RegisterType<LoginService>().As<ILoginService>();
                _builder.RegisterType<SearchService>().As<ISearchService>();
                _builder.RegisterType<BookService>().As<IBookService>();
                _builder.RegisterType<RecommenderService>().As<IRecommenderService>();

                _builder.RegisterType<SignUpService>().UsingConstructor(typeof(IRepository<User>));
                _builder.RegisterType<BookService>().UsingConstructor(typeof(IRepository<Book>),
                    typeof(IRepository<UserBookReview>), typeof(IRepository<ToRead>));
                _builder.RegisterType<ProfileService>().UsingConstructor(typeof(IRepository<User>),
                    typeof(IRepository<Connection>));

                _builder.RegisterType<BookwormDbContext>().As<IBookwormDbContext>();
            }

            Container = _builder.Build();
        }


        [Test]
        public void GivenValidConnectionDetails_WhenAddConnectionIsCalled_ReturnTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                Connection connection = new Connection()
                {
                    UserId = 289,
                    OtherUserId = 10
                };

                _profilesService = scope.Resolve<ProfileService>();

                // Act
                var result = _profilesService.AddConnection(connection);


                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<bool>());
                Assert.That(result, Is.EqualTo(true));
                Assert.IsTrue(_profilesService.DeleteConnection(connection)); // cleanup, undo database operations and assert it's been successful
            }
        }


        [Test]
        public void GivenValidConnectionDetails_WhenDeleteConnectionIsCalled_ReturnTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                Connection connection = new Connection()
                {
                    UserId = 289,
                    OtherUserId = 10
                };

                _profilesService = scope.Resolve<ProfileService>();

                _profilesService.AddConnection(connection);

                // Act
                var result = _profilesService.DeleteConnection(connection);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<bool>());
                Assert.That(result, Is.EqualTo(true));
                Assert.IsFalse(_profilesService.AreUsersConnected(connection)); // cleanup, undo database operations and assert it's been successful
            }
        }


        [Test]
        public void GivenConnectionDetailsOfUsersAlreadyConnected_WhenAreUsersConnectedIsCalled_ReturnTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                Connection connection = new Connection()
                {
                    UserId = 7,
                    OtherUserId = 10
                };

                _profilesService = scope.Resolve<ProfileService>();

                // Act
                var result = _profilesService.AreUsersConnected(connection);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<bool>());
                Assert.That(result, Is.EqualTo(true));
            }
        }


        [Test]
        public void GivenUserIdOfUserAlreadyInDb_WhenGetDetailsCalled_ReturnUser()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                int userId = 7;

                _profilesService = scope.Resolve<ProfileService>();

                // Act
                var result = _profilesService.GetUserDetails(userId);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<User>());
                Assert.That(result.FirstName, Is.EqualTo("Sean"));
                Assert.That(result.LastName, Is.EqualTo("Barker"));
            }
        }


        [Test]
        public void GivenUserIdOfUserNotInDb_WhenGetDetailsCalled_ReturnNull()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                int userId = -1;

                _profilesService = scope.Resolve<ProfileService>();

                // Act
                var result = _profilesService.GetUserDetails(userId);

                // Assert
                Assert.That(result, Is.Null);
            }
        }


        [Test]
        public void GivenUserIdAlreadyInDb_WhenGetAllOfAUsersConnectionsDetailsCalled_ReturnConnectionDetails()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                int userId = 7;

                _profilesService = scope.Resolve<ProfileService>();

                // Act
                var result = _profilesService.GetAllOfAUsersConnectionsDetails(userId);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<MyConnectionDetails>());
                Assert.That(result.ConnectionsProfileDetails, Is.Not.Null);
                Assert.That(result.Connections, Is.Not.Null);
                Assert.That(result.Connections.Count, Is.GreaterThan(0));
                Assert.That(result.Connections[0].OtherUserId, Is.EqualTo(10));
            }
        }


        [Test]
        public void GivenUserIdNotAlreadyInDb_WhenGetAllOfAUsersConnectionsDetailsCalled_ReturnEmptyConnectionDetails()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                int userId = -1;

                _profilesService = scope.Resolve<ProfileService>();

                // Act
                var result = _profilesService.GetAllOfAUsersConnectionsDetails(userId);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<MyConnectionDetails>());
                Assert.That(result.ConnectionsProfileDetails, Is.Not.Null);
                Assert.That(result.Connections, Is.Not.Null);
                Assert.That(result.Connections.Count, Is.EqualTo(0));
            }
        }

        
        [Test]
        public void GivenUserNotAlreadyInDb_WhenDeleteAccountCalled_ReturnTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                User user = _mockData.FakeUser2;
                UserSignUpViewModel signUpViewModel = new UserSignUpViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    City = user.City,
                    Email = user.Email,
                    Password = user.Password
                };

                _profilesService = scope.Resolve<ProfileService>();
                _signUpService = scope.Resolve<SignUpService>();


                // Act
                var result = _profilesService.DeleteUserAccount(user);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<bool>());
                Assert.That(result, Is.EqualTo(false));
            }
        }
    }
}