using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookworm.Models;
using Bookworm.ViewModels.Profiles;

namespace Bookworm.Contracts.Services
{
    public interface IBookService
    {
        // book related method signatures
        Book GetBookDetails(int bookId);
        bool UpdateBookRankings();
        List<Book> GetAllBooksDetailsForAUsersReviews(List<UserBookReview> bookReviews);

        // book review related method signatures
        UserBookReview GetABookReview(int userId, int bookId);
        List<UserBookReview> GetAllBookReviewsForBook(int bookId);
        List<UserBookReview> GetAllOfAUsersBookReviews(int userId);
        bool UserHasCreatedReview(int bookId, int userid);
        bool AddBookReview(UserBookReview userBookReview);
        bool DeleteBookReview(UserBookReview userBookReview);
        MyBookReviewsDetails GetAllOfAUsersBookReviewsDetails(int userId);

        // book shelf related method signatures
        bool AddBookToReadShelf(ToRead toRead);
        bool RemoveBookFromToRead(ToRead toRead);
        List<Book> GetBooksOnUsersBookShelf(int userId);
        bool IsOnBookShelf(ToRead bookToRead);

        
        void UpdateBookReview(UserBookReview userBookReview);

    }
}
