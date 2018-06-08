using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Profiles
{
    public class BookReviewWithBookInfo
    {
        public UserBookReview BookReview { get; set; }
        public Book BookDetails { get; set; }
    }
}