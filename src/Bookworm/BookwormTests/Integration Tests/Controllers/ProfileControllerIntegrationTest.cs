using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Integration.Mvc;
using Bookworm;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Controllers;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using Bookworm.Utils;
using Bookworm.ViewModels.Profiles;
using BookwormTests.MockData;
using FakeItEasy;
using NUnit.Framework;
using System.Web.SessionState;


namespace BookwormTests.Integration_Tests.Controllers
{
    [TestFixture]
    public class ProfileControllerIntegrationTest
    {
        private ProfilesController _profilesController;
        private String controllerName = "Profiles";
        private String actionCalledOtherUsersProfile = "OtherUsersProfile";
        private static ContainerBuilder _builder { get;  set; }
        private static Autofac.IContainer Container { get;  set; }

        
        [SetUp]
        public void SetUp()
        { 

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


        public bool undoCreateConnectionAdditionToDb(CombinedUserProfilesViewModel combinedUserDetails)
        {
            _profilesController.DeleteConnectionBetweenUsers(combinedUserDetails);
            return true;
        }


        [Test]
        public void GivenUsersAreConnected_WhenSetupOtherUsersProfileRequested_ReturnCombinedProfilesInPublicMode()
        {

            // Arrange
            var fakeConnection = A.Fake<Connection>();
            fakeConnection.UserId = 7;
            fakeConnection.OtherUserId = 10;

          
            using (var scope = Container.BeginLifetimeScope())
            {
                _profilesController = scope.Resolve<ProfilesController>();
 
                var fakeHttpContext = A.Fake<HttpContextBase>();
                var session = A.Fake<HttpSessionStateBase>();
                session["userId"] = 7;
                A.CallTo(() => fakeHttpContext.Session).Returns(session);
                ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                    _profilesController);

                _profilesController.ControllerContext = context;

                // Act
                var result = _profilesController.OtherUsersProfile(10) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.IsInstanceOf<CombinedUserProfilesViewModel>(result.Model);
                Assert.That(result.ViewBag.Message, Is.EqualTo("Other User's Profile"));
            }


        }


        [Test]
        public void GivenValidConnectionDetails_WhenCreateConnectionCalled_ReturnCorrectCombinedProfileViewModel()
        {
            // Arrange
            Connection coonection = new Connection();
            coonection.UserId = 5;
            coonection.OtherUserId = 10;

            CombinedUserProfilesViewModel combinedDetails = new CombinedUserProfilesViewModel();
            combinedDetails.Connection = coonection;
            using (var scope = Container.BeginLifetimeScope())
            {
                _profilesController = scope.Resolve<ProfilesController>();

                // Act
                var result = _profilesController.CreateConnectionBetweenUsers(combinedDetails) as RedirectToRouteResult;


                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;
                var userDetails = values.First();

                Assert.AreSame(userDetails, combinedDetails.MyUserDetails);
                Assert.IsTrue(values.Contains(CombinedUserProfilesViewModel.Success));
                Assert.IsTrue(values.Contains(controllerName));
                Assert.IsTrue(values.Contains(actionCalledOtherUsersProfile));

                Assert.IsTrue(undoCreateConnectionAdditionToDb(combinedDetails)); // undo creation addition to database
            }
        }


        [Test]
        public void GivenValidConnectionDetails_WhenDeleteConnectionCalled_ReturnCorrectCombinedProfileViewModel()
        {
            // Arrange
            Connection coonection = new Connection();
            coonection.UserId = 5;
            coonection.OtherUserId = 10;

            CombinedUserProfilesViewModel combinedDetails = new CombinedUserProfilesViewModel();
            combinedDetails.Connection = coonection;
            using (var scope = Container.BeginLifetimeScope())
            {
                _profilesController = scope.Resolve<ProfilesController>();
                _profilesController.CreateConnectionBetweenUsers(combinedDetails);

                // Act
                var result = _profilesController.DeleteConnectionBetweenUsers(combinedDetails) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;
                var userDetails = values.First();

                Assert.AreSame(userDetails, combinedDetails.MyUserDetails);
                Assert.IsTrue(values.Contains(CombinedUserProfilesViewModel.Success));
                Assert.IsTrue(values.Contains(controllerName));
                Assert.IsTrue(values.Contains(actionCalledOtherUsersProfile));
            }
        }

    }
}
 