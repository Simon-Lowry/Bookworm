using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Profiles;

namespace Bookworm.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<UserBookReview> _reviewsRepository;
        private readonly IRepository<ToRead> _toReadRepository;


        public BookService(IRepository<Book> bookRepo, IRepository<UserBookReview> reviewRepos,
            IRepository<ToRead> toReadRepo)
        {
            _bookRepository = bookRepo;
            _reviewsRepository = reviewRepos;
            _toReadRepository = toReadRepo;
        }


        public Book GetBookDetails(int bookId)
        {
            return _bookRepository.Get(bookId);
        }


        public UserBookReview GetABookReview(int userId, int bookId)
        {
            var bookReviews = (from userReviews in _reviewsRepository.GetListOf()
                where userReviews.BookId == bookId
                      && userReviews.UserId == userId
                select userReviews).ToList();

            UserBookReview bookReview = null;
            if (bookReviews.Count != 0)
                bookReview = bookReviews[0];

            return bookReview;   
        }


        public List<Book> GetAllBooksDetailsForAUsersReviews(List<UserBookReview> bookReviews)
        {
            List<Book> books = new List<Book>();

            foreach(UserBookReview bookReview in bookReviews)
            {
                int bookId = bookReview.BookId;
                Book book = GetBookDetails(bookId);
                books.Add(book);
            }

            return books.ToList();
        }


        public List<UserBookReview> GetAllBookReviewsForBook(int bookId)
        {
            var allBookReviewsForBook = from reviews in _reviewsRepository.GetListOf() where reviews.BookId == bookId select reviews;

            return allBookReviewsForBook.ToList();
        }


        public List<UserBookReview> GetAllOfAUsersBookReviews(int userId)
        {   
            var aUserBookReviews = from reviews in _reviewsRepository.GetListOf() where reviews.UserId == userId select reviews;

            return aUserBookReviews.ToList();
        }


        public bool UserHasCreatedReview(int bookId, int userId)
        {
            var allReviews = from reviews in _reviewsRepository.GetListOf() where reviews.BookId == bookId
                             && reviews.UserId == userId select reviews;

            return allReviews.ToList().Count == 1;
        }
        

        public bool AddBookReview(UserBookReview userBookReview)
        {
            if (UserHasCreatedReview(userBookReview.BookId, userBookReview.UserId))
                return false;

            return _reviewsRepository.Create(userBookReview);
        }


        public bool DeleteBookReview(UserBookReview userBookReview)
        {
           return _reviewsRepository.Delete(userBookReview);
        }


        public MyBookReviewsDetails GetAllOfAUsersBookReviewsDetails(int userId)
        {
            List<UserBookReview> myBookReviews = GetAllOfAUsersBookReviews(userId);
            List<Book> myBookReviewsBooks = GetAllBooksDetailsForAUsersReviews(myBookReviews);

            MyBookReviewsDetails myBookReviewsDetails = new MyBookReviewsDetails()
            {
                MyBookReviews = myBookReviews,
                MyBookReviewsBookDetails = myBookReviewsBooks
            };

            return myBookReviewsDetails;
        }


        public bool AddBookToReadShelf(ToRead bookToRead)
        {
            return _toReadRepository.Create(bookToRead);
        }


        public bool IsOnBookShelf(ToRead bookToRead)
        {
            var booksOnShelf = from booksToRead in _toReadRepository.GetListOf()  where booksToRead.BookId == bookToRead.BookId
                      && booksToRead.UserId == bookToRead.UserId
                             select booksToRead;

            return booksOnShelf.ToList().Count == 1;
        }


        public bool RemoveBookFromToRead(ToRead bookToRead)
        {
            return _toReadRepository.Delete(bookToRead);
        }


        public List<Book> GetBooksOnUsersBookShelf(int userId)
        {
            var idsOfBooksOnShelf = (from booksToRead in _toReadRepository.GetListOf()
                where booksToRead.UserId == userId select booksToRead).ToList();

            List<Book> booksOnUsersBookShelf = new List<Book>();

            for (int i = 0; i < idsOfBooksOnShelf.Count; i++)
            {
                var result = (from books in _bookRepository.GetListOf() where 
                    books.BookId == idsOfBooksOnShelf[i].BookId select books).ToList();

                booksOnUsersBookShelf.Add(result[0]);

            }
            
            return booksOnUsersBookShelf; 
        }


        // updates a book's average ratings after a book review has been added to the DB
        public bool UpdateBookRankings()
        {
            throw new NotImplementedException();
        }


        public void UpdateBookReview(UserBookReview userBookReview)
        {
             _reviewsRepository.Update(userBookReview);
        }
    }
}