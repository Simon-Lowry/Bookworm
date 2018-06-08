using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
using FakeItEasy;
using NUnit.Framework;

namespace BookwormTests.Integration_Tests.Controllers
{
    public class SignUpControllerIntegrationTest
    {
        private SignUpController _signUpController;
        private ProfileService _profileService;
        private String controllerName = "SignUp";
        private String actionCalledOtherUsersProfile = "SigningUp";
        private static ContainerBuilder _builder { get; set; }
        private static Autofac.IContainer Container { get; set; }

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


        [Test]
        public void GivenValidSignUpDetails_WhenSigningUpCalled_RedirectToProfile()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                _signUpController = scope.Resolve<SignUpController>();
                _profileService = scope.Resolve<ProfileService>();

                var userDetails = new UserSignUpViewModel();

                userDetails.FirstName = "Bill";
                userDetails.LastName = "Murray";
                userDetails.City = "Tokyo";
                userDetails.Country = "Japan";
                userDetails.Email = "abc@mail.com";
                userDetails.Password = "BillMurray1233";
                userDetails.ConfirmPassword = userDetails.Password;
                userDetails.DOB = "1990-10-10 00:00:00.000";

                var fakeHttpContext = A.Fake<HttpContextBase>();
                var session = A.Fake<HttpSessionStateBase>();
                session["userId"] = 7;
                A.CallTo(() => fakeHttpContext.Session).Returns(session);
                ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                    _signUpController);

                _signUpController.ControllerContext = context;

                // Act
                var result = _signUpController.SigningUp(userDetails) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;

                Assert.IsNotNull(values);
                Assert.IsTrue(values.Contains("Profiles"));
                Assert.IsTrue(values.Contains("MyProfile"));
            }
        }


        [Test]
        public void GivenInvalidSignUpDetails_WhenSigningUpCalled_RedirectToProfile()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                _signUpController = scope.Resolve<SignUpController>();
                _profileService = scope.Resolve<ProfileService>();

                var userDetails = new UserSignUpViewModel();
     
                // Act
                var result = _signUpController.SigningUp(userDetails) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.IsInstanceOf<UserSignUpViewModel>(result.Model);
                Assert.That(result.ViewBag.Message, Is.EqualTo("Error in SignUp Occured"));
            }
        }
    }
}