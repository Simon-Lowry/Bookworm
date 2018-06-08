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
using Bookworm.ViewModels.Books;
using Bookworm.ViewModels.Profiles;
using FakeItEasy;
using NUnit.Framework;

namespace BookwormTests.Integration_Tests.Controllers
{
    public class BooksControllerIntegrationTest
    {
        private BooksController _bookController;
        private String controllerName = "Books";
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
        public void GivenBookId_WhenBookProfileCalled_ReturnBookView()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookController = scope.Resolve<BooksController>();

                var fakeHttpContext = A.Fake<HttpContextBase>();
                var session = A.Fake<HttpSessionStateBase>();
                session["userId"] = 7;
                A.CallTo(() => fakeHttpContext.Session).Returns(session);
                ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                    _bookController);

                _bookController.ControllerContext = context;

                // Act
                var result = _bookController.BookProfile(11) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.IsInstanceOf<BookViewModel>(result.Model);
                Assert.That(result.ViewBag.Message, Is.EqualTo("Book Profile"));
            }
        }

        [Test]
        public void GivenValidToReadDetails_WhenAddBookToBookShelf_ReturnToBookProfile()
        {

            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookController = scope.Resolve<BooksController>();
                BookService bookService = scope.Resolve<BookService>();

                ToRead toRead = new ToRead();
                toRead.UserId = 289;
                toRead.BookId = 11;


                // Act
                var result = _bookController.AddBookToBookShelf(toRead) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;

                Assert.IsTrue(values.Contains(11));
                Assert.IsTrue(values.Contains(controllerName));
                Assert.IsTrue(values.Contains("BookProfile"));

                Assert.IsTrue(bookService.RemoveBookFromToRead(toRead)); // undo creation addition to database
            }
           
        }


        [Test]
        public void GivenValidToReadDetails_WhenDeleteBookFromBookShelf_ReturnToBookProfile()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookController = scope.Resolve<BooksController>();
                BookService bookService = scope.Resolve<BookService>();

                ToRead toRead = new ToRead();
                toRead.UserId = 289;
                toRead.BookId = 11;

                _bookController.AddBookToBookShelf(toRead);

                // Act
                var result = _bookController.DeleteBookFromBookShelf(toRead) as RedirectToRouteResult;
                
                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;

                Assert.IsTrue(values.Contains(11));
                Assert.IsTrue(values.Contains(controllerName));
                Assert.IsTrue(values.Contains("BookProfile"));
                Assert.IsNull(bookService.GetABookReview(289, 11)); // cleanup, assert it's been successful
            }
        }


        [Test]
        public void GivenValidBookReviewDetails_WhenCreateReviewCalled_ReturnToBookProfile()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookController = scope.Resolve<BooksController>();
                BookService bookService = scope.Resolve<BookService>();

                UserBookReview userBookReview = new UserBookReview();
                userBookReview.UserId = 289;
                userBookReview.BookId = 11;
                userBookReview.Rating = 5;
                userBookReview.Description = "Great book!";

                // Act
                var result = _bookController.CreateBookReview(userBookReview) as RedirectToRouteResult;

                UserBookReview userBookReviewResult = bookService.GetABookReview(289, 11); 

                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;

                Assert.IsTrue(values.Contains(11));
                Assert.IsTrue(values.Contains(controllerName));
                Assert.IsTrue(values.Contains("BookProfile"));
                Assert.AreEqual(userBookReviewResult.UserId, userBookReview.UserId);
                Assert.AreEqual(userBookReviewResult.BookId, userBookReview.BookId);
                Assert.IsTrue(bookService.DeleteBookReview(userBookReviewResult));    // cleanup, undo database operations and assert it's been successful
            }
        }


        [Test]
        public void GivenValidBookReviewDetails_WhenDeleteReviewCalled_ReturnToBookProfile()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookController = scope.Resolve<BooksController>();
                BookService bookService = scope.Resolve<BookService>();

                UserBookReview userBookReview = new UserBookReview();
                userBookReview.UserId = 289;
                userBookReview.BookId = 11;
                userBookReview.Rating = 5;
                userBookReview.Description = "Great book!";

                _bookController.CreateBookReview(userBookReview);
 
                // Act
                var result = _bookController.DeleteBookReview(userBookReview) as RedirectToRouteResult;
                
                // Assert
                Assert.IsNotNull(result);
                var values = result.RouteValues.Values;

                Assert.IsTrue(values.Contains(11));
                Assert.IsTrue(values.Contains(controllerName));
                Assert.IsTrue(values.Contains("BookProfile"));
                Assert.IsNull(bookService.GetABookReview(289, 11)); // cleanup, assert it's been successful
            }
        }
    }
}