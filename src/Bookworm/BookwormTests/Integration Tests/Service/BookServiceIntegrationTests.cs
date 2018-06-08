using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Bookworm;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using Bookworm.ViewModels.Profiles;
using BookwormTests.MockData;
using NUnit.Framework;

namespace BookwormTests.Integration_Tests.Service
{
    public class BookServiceIntegrationTests
    {
        private BookService _bookService; 
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
        public void GivenValidBookId_WhenGetBookDetailsCalled_ReturnBook()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();

                // Act
                var result = _bookService.GetBookDetails(1);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<Book>());
                Assert.IsTrue(result.Authors.Equals("J.K. Rowling"));
            }
        }


        [Test]
        public void GivenValidBookIdAndUserId_WhenGetABookReviewCalled_ReturnBookReview()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
                int bookId = 1;
                int userId = 588;

                var result = _bookService.GetABookReview(userId, bookId);

                Assert.IsNotNull(result);
                Assert.AreEqual(result.BookId, 1);
                Assert.AreEqual(result.ReviewId, 3);
            }
        }

        [Test]
        public void GivenValidBookAndUserIds_WhenUserHasCreatedReviewCalled_ReturnTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
                int bookId = 1;
                int userId = 588;

                var result = _bookService.UserHasCreatedReview(bookId, userId);

                Assert.IsNotNull(result);
                Assert.AreEqual(result, true);
            }
        }


        [Test]
        public void GivenToRead_WhenIsBookOnToReadShelf_ReturnTrue()
        {
            ToRead toRead = new ToRead();
            toRead.BookId = 11;
            toRead.UserId = 289;
           
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
               

                var result = _bookService.IsOnBookShelf(toRead);

                Assert.IsNotNull(result);
                Assert.AreEqual(result, true);
            }

        }


        [Test]
        public void GivenValidUserId_WhenGetAllOfAUsersBookReviewsDetailsCalled_ReturnMyBookReviewDetails()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
                int userId = 588;
               
                // Act
                var result = _bookService.GetAllOfAUsersBookReviewsDetails(userId);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<MyBookReviewsDetails>());
                Assert.That(result.MyBookReviews, Is.InstanceOf<List<UserBookReview>>());
                Assert.That(result.MyBookReviewsBookDetails, Is.InstanceOf<List<Book>>());
            }
        }


        [Test]
        public void GivenValidUserId_WhenGetAllOfAUsersBookReviews_ReturnListOfReviews()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
                int userId = 588;
               
                // Act
                var result = _bookService.GetAllOfAUsersBookReviews(userId);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<List<UserBookReview>>());
                Assert.That(result.Count, Is.GreaterThan(0));
            }
        }


        [Test]
        public void GivenValidUserId_WhenGetBooksOnUsersBookHself_ReturnListOfBooks()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
                int userId = 588;
               
                // Act
                var result = _bookService.GetBooksOnUsersBookShelf(userId);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<List<Book>>());
            }
        }


        [Test]
        public void GivenValidUserId_WhenGetAllReviewsForBook_ReturnListOfBookReviews()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _bookService = scope.Resolve<BookService>();
                int userId = 588;
               
                // Act
                var result = _bookService.GetAllBookReviewsForBook(1);

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<List<UserBookReview>>());
                Assert.That(result.Count, Is.GreaterThan(0));

            }
        }


    }
}