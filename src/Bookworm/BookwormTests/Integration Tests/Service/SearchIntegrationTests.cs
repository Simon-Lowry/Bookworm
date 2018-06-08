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
using BookwormTests.MockData;
using NUnit.Framework;

namespace BookwormTests.Integration_Tests.Service
{
    public class SearchIntegrationTests
    {
         private SearchService _searchService; 
        private static ContainerBuilder _builder { get; set; }
        private static IContainer Container { get; set; }
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
                _builder.RegisterType<SearchService>().UsingConstructor(typeof(IRepository<User>),
                    typeof(IRepository<Book>));

                _builder.RegisterType<BookwormDbContext>().As<IBookwormDbContext>();
            }

            Container = _builder.Build();
        }

        [Test]
        public void GivenValidSearchString_WhenSearchForUserCalled_ReturnListOfUsers()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _searchService = scope.Resolve<SearchService>();
                int userId = 588;
               
                // Act
                var result = _searchService.SearchForUsers("Sean Barker");

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<List<User>>());
                Assert.That(result.Count, Is.GreaterThan(0));
            }
        }

        [Test]
        public void GivenInValidSearchString_WhenSearchForUserCalled_ReturnListOfUsers()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _searchService = scope.Resolve<SearchService>();
               
                // Act
                var result = _searchService.SearchForUsers("Barwefweewewker");

                // Assert
                Assert.That(result, Is.Null);
            }
        }


        [Test]
        public void GivenValidBookString_WhenSearchForBookCalled_ReturnListOfBooks()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _searchService = scope.Resolve<SearchService>();
                

                // Act
                var result = _searchService.SearchForBooks("A Short History of Nearly Everything");

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<List<Book>>());
                Assert.That(result.Count, Is.GreaterThan(0));
            }
        }


        [Test]
        public void GivenInValidBookString_WhenSearchForBookCalled_ReturnListOfBooks()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // Arrange
                _searchService = scope.Resolve<SearchService>();
                

                // Act
                var result = _searchService.SearchForBooks("iwefrefrstory");

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<List<Book>>());
                Assert.That(result.Count, Is.LessThan(1));
            }
        }
    }
}