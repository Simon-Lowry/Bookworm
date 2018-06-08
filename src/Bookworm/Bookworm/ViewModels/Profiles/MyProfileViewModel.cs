using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Bookworm.ViewModels.Profiles
{
    public class MyProfileViewModel
    {
        public User MyDetails { get; set; }
        public MyBookReviewsDetails MyBookReviews { get; set; }
        public MyConnectionDetails MyConnections { get; set; }
        public List<Book> MyToReadBookDetails { get; set; }

    }
}