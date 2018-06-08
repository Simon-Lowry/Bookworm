using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bookworm.Contracts;
using FakeItEasy;
using NUnit.Framework;
using Bookworm.Controllers;
using Bookworm.Contracts.Services;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.ViewModels.Books;
using Bookworm.ViewModels.Profiles;

namespace BookwormTests.Unit_Tests.Controllers
{
    [TestFixture]
    public class BookControllerUnitTests
    {
        private BooksController _bookController;
        private IBookService _fakeBookService;
        private IProfileService _fakeProfileService;
        private IRepository<User> _fakeRepository;
        private String controllerName = "Books";
        private String actionCalledBookProfile = "BookProfile";

        [SetUp]
        public void SetUp()
        {
            _fakeBookService = A.Fake<IBookService>();
            var _fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _bookController = new BooksController(_fakeBookService, _fakeProfileService);
            _fakeRepository = new Repository<User>(_fakeBookwormDbContext);
        }


        [Test]
        public void GivenUserBookReview_WhenCreateBookReviewCalled_RedirectWithCorrectDetails()
        {
            // Arrange
            var fakeBookReview = A.Fake<UserBookReview>();
            fakeBookReview.BookId = 12;
            fakeBookReview.UserId = 10;

            A.CallTo(() => _fakeBookService.AddBookReview(fakeBookReview)).Returns(true);

            // Act
            var result = _bookController.CreateBookReview(fakeBookReview) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var bookId = values.ElementAt(0);
            var userId = values.ElementAt(1);

            Assert.AreEqual(bookId, fakeBookReview.BookId);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledBookProfile));

            A.CallTo(() => _fakeBookService.AddBookReview(fakeBookReview)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenUserBookReview_WhenDeleteBookReviewCalled_RedirectWithCorrectDetails()
        {
            // Arrange
            var fakeBookReview = A.Fake<UserBookReview>();
            fakeBookReview.BookId = 12;
            fakeBookReview.UserId = 10;

            A.CallTo(() => _fakeBookService.DeleteBookReview(fakeBookReview)).Returns(true);

            // Act
            var result = _bookController.DeleteBookReview(fakeBookReview) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var bookId = values.ElementAt(0);
            var userId = values.ElementAt(1);

            Assert.AreEqual(bookId, fakeBookReview.BookId);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledBookProfile));

            A.CallTo(() => _fakeBookService.DeleteBookReview(fakeBookReview)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GivenBookId_WhenBookProfileCalled_ReturnBookView()
        {
            // Arrange
            int bookId = 1;
            int userId = 10;

            var book = A.Fake<Book>();
            book.BookId = 1;

            var fakeHttpContext = A.Fake<HttpContextBase>();
            var session = A.Fake<HttpSessionStateBase>();
            session["userId"] = 7;
            A.CallTo(() => fakeHttpContext.Session).Returns(session);
            ControllerContext context = new ControllerContext(new RequestContext(fakeHttpContext, new RouteData()),
                _bookController);

            _bookController.ControllerContext = context;

            A.CallTo(() => _fakeBookService.GetBookDetails(bookId)).Returns(book);
            A.CallTo(() => _fakeBookService.UserHasCreatedReview(bookId, userId)).Returns(true);
            A.CallTo(() => _fakeBookService.IsOnBookShelf(A<ToRead>._)).Returns(true);

            // Act
            var result = _bookController.BookProfile(bookId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Book Profile"));
            A.CallTo(() => _fakeBookService.GetBookDetails(bookId)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidToReadDetails_WhenDeleteFromBookShelfCalled_ReturnToBookProfile()
        {
            // Arrange
            var fakeBookToRead = A.Fake<ToRead>();
            fakeBookToRead.BookId = 1;
            fakeBookToRead.UserId = 10;

            A.CallTo(() => _fakeBookService.RemoveBookFromToRead(fakeBookToRead)).Returns(true);

            // Act
            var result = _bookController.DeleteBookFromBookShelf(fakeBookToRead) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var bookId = values.ElementAt(0);

            Assert.AreEqual(bookId, fakeBookToRead.BookId);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledBookProfile));

            A.CallTo(() => _fakeBookService.RemoveBookFromToRead(fakeBookToRead)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GivenInvalidToReadDetails_WhenAddToBookShelfCalled_ReturnToBookProfile()
        {
            // Arrange
            var fakeBookToRead = A.Fake<ToRead>();
            fakeBookToRead.BookId = -1;
            fakeBookToRead.UserId = -10;

            A.CallTo(() => _fakeBookService.AddBookToReadShelf(fakeBookToRead)).Returns(false);

            // Act
            var result = _bookController.AddBookToBookShelf(fakeBookToRead) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var bookId = values.ElementAt(0);

            Assert.AreEqual(bookId, fakeBookToRead.BookId);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledBookProfile));

            A.CallTo(() => _fakeBookService.AddBookToReadShelf(fakeBookToRead)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidToReadDetails_WhenAddToBookShelfCalled_ReturnToBookProfile()
        {
            // Arrange
            var fakeBookToRead = A.Fake<ToRead>();
            fakeBookToRead.BookId = 1;
            fakeBookToRead.UserId = 10;

            A.CallTo(() => _fakeBookService.AddBookToReadShelf(fakeBookToRead)).Returns(true);

            // Act
            var result = _bookController.AddBookToBookShelf(fakeBookToRead) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var bookId = values.ElementAt(0);

            Assert.AreEqual(bookId, fakeBookToRead.BookId);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledBookProfile));

            A.CallTo(() => _fakeBookService.AddBookToReadShelf(fakeBookToRead)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GivenInvalidToReadDetails_WhenDeleteFromBookShelfCalled_ReturnToBookProfile()
        {
            // Arrange
            var fakeBookToRead = A.Fake<ToRead>();
            fakeBookToRead.BookId = -1;
            fakeBookToRead.UserId = -10;

            A.CallTo(() => _fakeBookService.RemoveBookFromToRead(fakeBookToRead)).Returns(false);

            // Act
            var result = _bookController.DeleteBookFromBookShelf(fakeBookToRead) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var values = result.RouteValues.Values;
            var bookId = values.ElementAt(0);

            Assert.AreEqual(bookId, fakeBookToRead.BookId);
            Assert.IsTrue(values.Contains(controllerName));
            Assert.IsTrue(values.Contains(actionCalledBookProfile));

            A.CallTo(() => _fakeBookService.RemoveBookFromToRead(fakeBookToRead)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}