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
using BookwormTests.MockData;
using FakeItEasy;
using NUnit;
using NUnit.Framework;

namespace BookwormTests.Integration_Tests.Controllers
{
    public class LoginControllerIntegrationTest
    {
        private LoginController _loginController;
        private String controllerName = "Login";
        private String viewBagMessage = "Error Message";
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
        public void GivenGoodCredentials_WhenAttemptLoginCalled_ReturnToLoginPageWithError()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                _loginController = scope.Resolve<LoginController>();

                String email = "abc5575@gmail.com";
                String password = "Ur12355,";

                UserLoginViewModel userLoginDetails = new UserLoginViewModel()
                {
                    Email = email,
                    Password = password
                };

                var fakeHttpContext = A.Fake<HttpContextBase>();
                var session = A.Fake<HttpSessionStateBase>();
                session["userId"] = 7;
                A.CallTo(() => fakeHttpContext.Session).Returns(session);
                ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                    _loginController);

                _loginController.ControllerContext = context;

                // Act
                var result = _loginController.AttemptLogin(userLoginDetails) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;

                Assert.IsNotNull(values);
                Assert.IsTrue(values.Contains("Profiles"));
                Assert.IsTrue(values.Contains("MyProfile"));
            }
            
        }

        [Test]
        public void GivenBadCredentials_WhenAttemptLoginCalled_ReturnToLoginPageWithError()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                _loginController = scope.Resolve<LoginController>();

                String email = "abc5575@gmail.com";
                String password = "jick,";

                UserLoginViewModel userLoginDetails = new UserLoginViewModel()
                {
                    Email = email,
                    Password = password
                };

                // Act
                var result = _loginController.AttemptLogin(userLoginDetails) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.IsInstanceOf<UserLoginViewModel>(result.Model);
                Assert.That(result.ViewBag.Message, Is.EqualTo("Error Occured"));
            }
        }

    }
}