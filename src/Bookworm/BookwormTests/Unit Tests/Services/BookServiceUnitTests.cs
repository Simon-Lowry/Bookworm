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
using NUnit.Framework;
using FakeItEasy;
using BookwormTests.MockData;

namespace BookwormTests.Unit_Tests.Services
{
    [TestFixture]
    public class BookServiceUnitTest
    {
        private IRepository<Book> _bookRepository;
        private IRepository<UserBookReview> _reviewsRepository;
        private IRepository<ToRead> _toReadRepository;
        private MockUsers _mockUserData;
        private IBookService _bookService;


        [SetUp]
        public void SetUp()
        {
            var fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _bookRepository = A.Fake<Repository<Book>>();
            _reviewsRepository = A.Fake<Repository<UserBookReview>>();
            _toReadRepository = A.Fake<Repository<ToRead>>();
            _mockUserData = new MockUsers();
            _bookService = new BookService(_bookRepository, _reviewsRepository, _toReadRepository);
        }


        [Test]
        public void GivenValidBookId_WhenGetBookDetailsCalled_ReturnBook()
        {
            int bookId = 1;
            var book = A.Fake<Book>();

            A.CallTo(() => _bookRepository.Get(bookId)).Returns(book);

            var result = _bookService.GetBookDetails(bookId);

            Assert.IsNotNull(result);
            Assert.That(result, Is.InstanceOf<Book>());
        }


        [Test]
        public void GivenValidBookId_WhenGetAllBookReviewsForBookCalled_ReturnBookReviewList()
        {
            int bookId = 1;
            var book = A.Fake<Book>();

            A.CallTo(() => _bookRepository.Get(bookId)).Returns(book);

            var fakeReview = A.Fake<UserBookReview>();
            fakeReview.UserId = 1;
            fakeReview.Rating = 2;

            var fakeUserBookReview2 = A.Fake<UserBookReview>();
            fakeUserBookReview2.BookId = 1;
            fakeUserBookReview2.UserId = -2;


            IList<UserBookReview> reviews = A.Fake<List<UserBookReview>>();

            reviews.Add(fakeReview);
            reviews.Add(fakeUserBookReview2);

            A.CallTo(() => _reviewsRepository.GetListOf()).Returns(reviews);

            var result = _bookService.GetAllBookReviewsForBook(bookId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IList<UserBookReview>>());
            A.CallTo(() => _reviewsRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidBookId_WhenGetAllOfAUsersBookReviewsCalled_ReturnBookReviewList()
        {
            int bookId = 1;
            var book = A.Fake<Book>();

            A.CallTo(() => _bookRepository.Get(bookId)).Returns(book);

            var fakeReview = A.Fake<UserBookReview>();
            fakeReview.UserId = 1;
            fakeReview.Rating = 2;

            var fakeUserBookReview2 = A.Fake<UserBookReview>();
            fakeUserBookReview2.BookId = 1;
            fakeUserBookReview2.UserId = -2;


            IList<UserBookReview> reviews = A.Fake<List<UserBookReview>>();

            reviews.Add(fakeReview);
            reviews.Add(fakeUserBookReview2);

            A.CallTo(() => _reviewsRepository.GetListOf()).Returns(reviews);

            var result = _bookService.GetAllOfAUsersBookReviews(bookId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IList<UserBookReview>>());
            A.CallTo(() => _reviewsRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidBookReview_WhenAddBookReviewCalled_ReturnTrue()
        {
            int bookId = 1;
            var bookReview = A.Fake<UserBookReview>();


            A.CallTo(() => _reviewsRepository.Create(bookReview)).Returns(true);

            var result = _bookService.AddBookReview(bookReview);

            Assert.IsNotNull(result);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }


        [Test]
        public void GivenValidBookReview_WhenDeleteBookReviewCalled_ReturnTrue()
        {
            int bookId = 1;
            var bookReview = A.Fake<UserBookReview>();

            A.CallTo(() => _reviewsRepository.Delete(bookReview)).Returns(true);

            var result = _bookService.DeleteBookReview(bookReview);

            Assert.IsNotNull(result);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }


        [Test]
        public void GivenValidToRead_WhenAddBookToBookShelfCalled_ReturnTrue()
        {
            int bookId = 1;
            var toRead = A.Fake<ToRead>();


            A.CallTo(() => _toReadRepository.Create(toRead)).Returns(true);

            var result = _bookService.AddBookToReadShelf(toRead);

            Assert.IsNotNull(result);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public void GivenValidToRead_WhenDeleteBookFromBookShelfCalled_ReturnTrue()
        {
            int bookId = 1;
            var toRead = A.Fake<ToRead>();

            A.CallTo(() => _toReadRepository.Delete(toRead)).Returns(true);

            var result = _bookService.RemoveBookFromToRead(toRead);

            Assert.IsNotNull(result);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

    }
}