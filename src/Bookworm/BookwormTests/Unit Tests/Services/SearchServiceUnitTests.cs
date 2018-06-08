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
using BookwormTests.MockData;
using FakeItEasy;
using NUnit.Framework;

namespace BookwormTests.Unit_Tests.Services
{
    [TestFixture]
    public class SearchServiceUnitTest
    {
        private IRepository<User> _userRepository;
        private IRepository<Book> _bookRepository;
        private MockUsers _mockUserData;
        private ISearchService _searchService;


        [SetUp]
        public void SetUp()
        {
            var fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _userRepository = A.Fake<Repository<User>>();
            _bookRepository = A.Fake<Repository<Book>>();
            _mockUserData = new MockUsers();
            _searchService = new SearchService(_userRepository, _bookRepository);
        }


        [Test]
        public void GivenValidUsername_WhenSearchForUsersCalled_ReturnUserInList()
        {
            // Arrange
            var username = "Simon Lowry";

            List<User> users = A.Fake<List<User>>();
            var fakeUser = A.Fake<User>();
            fakeUser.FirstName = "Simon";
            fakeUser.LastName = "Lowry";
            users.Add(fakeUser);

            A.CallTo(() => _userRepository.GetListOf()).Returns(users);

            // Act
            var result = _searchService.SearchForUsers(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count == 1);
            Assert.AreSame(result[0], fakeUser);
            A.CallTo(() => _userRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void GivenValidBookEntry_WhenSearchForBooksCalled_ReturnBookInList()
        {
            // Arrange
            var bookTitle = "Harry Potter and the Chamber of Secrets";

            List<Book> books = A.Fake<List<Book>>();
            var fakeBook = A.Fake<Book>();
            fakeBook.Title = bookTitle;
            books.Add(fakeBook);

            A.CallTo(() => _bookRepository.GetListOf()).Returns(books);

            // Act
            var result = _searchService.SearchForBooks(bookTitle);

            // Assert
            Assert.That(result.Count == 1);
            A.CallTo(() => _bookRepository.GetListOf()).MustHaveHappened(Repeated.Exactly.Once);
        }

        
    }
}