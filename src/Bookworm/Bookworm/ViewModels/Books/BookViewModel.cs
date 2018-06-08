using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Books
{
    public class BookViewModel
    {
       public Book BookDetails { get; set; }
       public UserBookReview BookReview { get; set; }
       public List<UserBookReview> AllReviewsForBook { get; set; }
       public ToRead BookToRead { get; set; }
       public bool HasCreatedReview { get; set; }
       public bool IsOnBookShelf { get; set; }
    }
}