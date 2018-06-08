using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Books;
using Microsoft.Ajax.Utilities;

namespace Bookworm.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IProfileService _profileService;


        public BooksController(IBookService bookService, IProfileService profileService)
        {
            _bookService = bookService;
            _profileService = profileService;
        }


        public ActionResult BookProfile(int bookId)
        {
            Book book = _bookService.GetBookDetails(bookId);
            int userId = Convert.ToInt32(Session["userId"]);

            BookViewModel bookViewModel = new BookViewModel()
            {
                BookDetails = book,
                BookReview = new UserBookReview()
                {
                    UserId = userId
                },
                BookToRead = new ToRead()
                {
                    UserId = userId,
                    BookId = bookId,
                }
            };

            bookViewModel = SetupBookReviewModel(bookViewModel, userId);
            ViewBag.Message = "Book Profile";
            return View(bookViewModel);
        }

        // gets the associated user details with a book review
        public BookViewModel SetupBookReviewModel(BookViewModel bookViewModel, int userId)
        {
            int bookId = bookViewModel.BookDetails.BookId;

            bookViewModel.HasCreatedReview = _bookService.UserHasCreatedReview(bookId, userId);
            bookViewModel.IsOnBookShelf = _bookService.IsOnBookShelf(bookViewModel.BookToRead);
            bookViewModel.AllReviewsForBook = _bookService.GetAllBookReviewsForBook(bookId);

            if (bookViewModel.HasCreatedReview)
            {
                bookViewModel.BookReview = _bookService.GetABookReview(userId, bookId);
            }
            foreach (UserBookReview review in bookViewModel.AllReviewsForBook)
            {
                review.User = _profileService.GetUserDetails(review.UserId);
            }

            return bookViewModel;
        }

        [HttpPost]
        public ActionResult CreateBookReview( UserBookReview userBookReview)
        {
            bool hasReviewBeenCreated = _bookService.AddBookReview(userBookReview);
            
            return RedirectToAction("BookProfile", "Books", new { bookId = userBookReview.BookId } );
        }


        [HttpPost]
        public ActionResult UpdateBookReview( UserBookReview userBookReview)
        {
            _bookService.UpdateBookReview(userBookReview);
            
            return RedirectToAction("BookProfile", "Books", new { bookId = userBookReview.BookId } );
        }

        [HttpPost]
        public ActionResult DeleteBookReview(UserBookReview bookReview)
        {
            bool hasReviewBeenDeleted = _bookService.DeleteBookReview(bookReview);

            return RedirectToAction("BookProfile", "Books", new { bookId = bookReview.BookId });
        }


        public ActionResult AddBookToBookShelf(ToRead bookToRead)
        {
            bool isBookAddedToShelf = _bookService.AddBookToReadShelf(bookToRead);

            ViewBag.Message = "Book Profile";
            return RedirectToAction("BookProfile", "Books", new { bookId = bookToRead.BookId });
        }


        public ActionResult DeleteBookFromBookShelf(ToRead bookToRead)
        {
            bool isBookRemoved = _bookService.RemoveBookFromToRead(bookToRead);

            ViewBag.Message = "Book Profile";
            return RedirectToAction("BookProfile", "Books", new { bookId = bookToRead.BookId});
        }

        [HttpGet]
        public PartialViewResult ReadOnlyBookReviewModal(BookViewModel bookViewModel)
        {
           return PartialView("~/Views/PartialViews/ReadOnlyBookReviewModal.cshtml", bookViewModel);
        }
        

    }
}