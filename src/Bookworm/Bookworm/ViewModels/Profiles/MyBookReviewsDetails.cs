using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Profiles
{
    public class MyBookReviewsDetails
    {
        public List<UserBookReview> MyBookReviews { get; set; }
        public List<Book> MyBookReviewsBookDetails { get; set; }
    }
}