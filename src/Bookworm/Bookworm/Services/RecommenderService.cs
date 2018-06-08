using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Data;
using Bookworm.Models;
using System.Globalization;
using System.Net.Cache;
using System.Web.DynamicData;
using System.Web.Mvc.Html;
using System.Web.WebSockets;
using NReco.CF.Taste.Impl.Common;
using NReco.CF.Taste.Impl.Model;
using NReco.CF.Taste.Impl.Neighborhood;
using NReco.CF.Taste.Impl.Recommender;
using NReco.CF.Taste.Impl.Recommender.SVD;
using NReco.CF.Taste.Impl.Similarity;
using NReco.CF.Taste.Model;
using WebGrease.Css;
using NReco.CF.Taste.Similarity;
using NReco.CF.Taste.Neighborhood;
using NReco.CF.Taste.Recommender;

namespace Bookworm.Services
{
    public class RecommenderService : IRecommenderService
    {
        private const string connectionString = @"Data Source=(LocalDb)/MSSQLLocalDB; AttachDbFilename=C:\Dev\BookWorm\2018-ca400-lowrys5\src\Bookworm\Bookworm\App_Data\IBookwormDbContext.mdf; User Instance=True; Integrated Security=True";
        private const int NUM_RECOMMENDATIONS = 12;
        private const int NUM_USERS = 5;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<UserBookReview> _userBookReviewRepository;

        public RecommenderService(IRepository<Book> bookRepository, IRepository<UserBookReview> reviewRepo)
        {
            _bookRepository = bookRepository;
            _userBookReviewRepository = reviewRepo;
        }


        public List<int> GetRecommendations(int userId)
        {
            GenericDataModel model = GetUserBasedDataModel();

            EuclideanDistanceSimilarity similarity = new EuclideanDistanceSimilarity(
                model);

            IUserNeighborhood neighborhood = new NearestNUserNeighborhood(
                15, similarity, model);

            long[] neighbors = neighborhood.GetUserNeighborhood(userId);

            var recommender =
                new GenericUserBasedRecommender(model, neighborhood, similarity);
            var recommendedItems = recommender.Recommend(userId, 12);

            var bookIds = recommendedItems.Select(ri => (int)ri.GetItemID()).ToList();
           
            return bookIds;
        }
  
        public List<Book> GetBooksRecommended(List<int> bookIds)
        {
           return (from book in _bookRepository.GetAll()
                where bookIds.Contains(book.BookId)
                select new Book
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Authors = book.Authors,
                    Language = book.Language,
                    Isbn = book.Isbn,
                    PublishedYear = book.PublishedYear,
                    BookImgUrl = book.BookImgUrl,

                    AvgRating = book.AvgRating,
                    Ratings_1 = book.Ratings_1,
                    Ratings_2 = book.Ratings_2,
                    Ratings_3 = book.Ratings_3,
                    Ratings_4 = book.Ratings_4,
                    Ratings_5 = book.Ratings_5
                }).ToList();
        }


        public long[] GetNNearestNeighborsUsersRecommendations(int numNeighbours, int userId)
        {
            GenericDataModel model = GetUserBasedDataModel();

            EuclideanDistanceSimilarity similarity = new EuclideanDistanceSimilarity(
                model);

            IUserNeighborhood neighborhood = new NearestNUserNeighborhood(
                20, similarity, model);

            long[] neighbors = neighborhood.GetUserNeighborhood(userId);

            var recommender =
                new GenericUserBasedRecommender(model, neighborhood, similarity);
            var recommendedItems = recommender.Recommend(userId, 8);

            return neighbors;
        }


        public GenericDataModel GetItemBasedDataModel()
        {
            FastByIDMap<IPreferenceArray> data = new FastByIDMap<IPreferenceArray>();

            IEnumerable<UserBookReview> allBookReviews = _userBookReviewRepository.GetListOf();

            var everyReviewsBookId = allBookReviews.GroupBy(b => b.BookId).Select(x => x.Key).ToList();


            foreach (int bookId in everyReviewsBookId)
            {
                List<UserBookReview> bookReviewsForABook = (from userReviews in allBookReviews
                    where userReviews.BookId == bookId
                    select userReviews).ToList();
                List<IPreference> listOfPreferences = new List<IPreference>();

                foreach (UserBookReview review in bookReviewsForABook)
                {
                    int rating = review.Rating;
                    int reviewUserId = review.UserId;
                    GenericPreference pref = new GenericPreference(reviewUserId, bookId, rating); /// userId,  itemid, valueId

                    listOfPreferences.Add(pref);
                }

                GenericItemPreferenceArray dataArray = new GenericItemPreferenceArray(listOfPreferences);
                data.Put(bookId, dataArray);
            }

            return new GenericDataModel(data);
        }


        public GenericDataModel GetUserBasedDataModel()
        {
            FastByIDMap<IPreferenceArray> data = new FastByIDMap<IPreferenceArray>();

            IEnumerable<UserBookReview> allBookReviews = _userBookReviewRepository.GetListOf();

            var everyReviewsUserId = allBookReviews.GroupBy(b => b.UserId).Select(x => x.Key).ToList();

            foreach (int userId in everyReviewsUserId)
            {
                List<UserBookReview> bookReviewsForABook = (from userReviews in allBookReviews
                    where userReviews.UserId == userId
                                                            select userReviews).ToList();
                List<IPreference> listOfPreferences = new List<IPreference>();

                foreach (UserBookReview review in bookReviewsForABook)
                {
                    int rating = review.Rating;
                    int bookId = review.BookId;
                    GenericPreference pref = new GenericPreference(userId, bookId, rating); /// userId,  itemid, valueId

                    listOfPreferences.Add(pref);
                }

                GenericUserPreferenceArray dataArray = new GenericUserPreferenceArray(listOfPreferences);
                data.Put(userId, dataArray);
            }

            return new GenericDataModel(data);
        }


        public List<Book> GetBooksFromAuthorsAUserLiked(int userId)
        {
            List<Book> booksByAuthorsUserLikesNotReviewed = new List<Book>();
            List<string> authorsLikedByUser = GetAuthorsLikedByUser(userId);
               
            foreach (String authors in authorsLikedByUser )
            {
                List<Book> booksByAuthor = (from books in _bookRepository.GetListOf()
                    
                    where books.Authors == authors 
                    select books).Distinct().ToList(); //  && books.BookId != review.BookId && review.UserId != userId
               
                List<Book> booksNotReviewedByAuthorList = GetBooksNotReviewedByAuthor( userId, booksByAuthor);
                booksByAuthorsUserLikesNotReviewed.AddRange(booksNotReviewedByAuthorList);

            }

            return booksByAuthorsUserLikesNotReviewed;
        }


        public List<string> GetAuthorsLikedByUser(int userId)
        {
            List<string> authorsLikedByUser = (from books in _bookRepository.GetListOf()       // gets authors liked by user (authors which the user has rated 3 stars or more)
                join review in _userBookReviewRepository.GetListOf() on books.BookId equals review.BookId
                where review.UserId == userId && review.Rating >= 3
                select books.Authors).ToList();
            authorsLikedByUser = authorsLikedByUser.Distinct().ToList();   // gets unique author names liked by user

            return authorsLikedByUser;
        }


        public List<Book> GetBooksNotReviewedByAuthor(int userId, List<Book> booksByAuthor)
        {
            List<Book> booksNotReviewedByUser = new List<Book>();
            List<int> userBookReviewsBookIdList = (from reviews in _userBookReviewRepository.GetListOf() where reviews.UserId == userId select reviews.BookId).ToList();

            foreach (Book book in booksByAuthor)
            {
                int bookId = book.BookId;

                if (!userBookReviewsBookIdList.Contains(bookId))
                    booksNotReviewedByUser.Add(book);
            }

            return booksNotReviewedByUser;
        }
    }
}