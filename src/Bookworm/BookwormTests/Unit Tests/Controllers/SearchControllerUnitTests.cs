using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Controllers;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using FakeItEasy;
using NUnit.Framework;

namespace BookwormTests.Unit_Tests.Controllers
{
    [TestFixture]
    public class SearchControllerUnitTests
    {
        private SearchController _searchController;
        private ISearchService _fakeSearchService;
        private IRepository<User> _fakeRepository;

        [SetUp]
        public void SetUp()
        {
            _fakeSearchService = A.Fake<ISearchService>();
            var _fakeBookwormDbContext = A.Fake<BookwormDbContext>();
            _searchController = new SearchController(_fakeSearchService);
            _fakeRepository = new Repository<User>(_fakeBookwormDbContext);
        }

        [Test]
        public void GivenSearchQueryPresent_WhenSearched_ReturnSuccess()
        {
            // Arrange
            var fakeSearchQuery = "MySearch";

            List<Book> fakeBookList = new List<Book>();
            var fakeBook = A.Fake<Book>();
            fakeBook.Authors = "bill buck";
            fakeBook.Title = "This book";

            fakeBookList.Add(fakeBook);

            List<User> fakeUserList = new List<User>();
            A.CallTo(() => _fakeSearchService.SearchForBooks(fakeSearchQuery)).Returns(fakeBookList);
            A.CallTo(() => _fakeSearchService.SearchForUsers(fakeSearchQuery)).Returns(fakeUserList);

            // Act
            ViewResult result = _searchController.Search(fakeSearchQuery) as ViewResult ;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ViewBag.Message, Is.EqualTo("Search View"));

            A.CallTo(() => _fakeSearchService.SearchForUsers(fakeSearchQuery)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _fakeSearchService.SearchForBooks(fakeSearchQuery)).MustHaveHappened(Repeated.Exactly.Once);
        }


    }
}