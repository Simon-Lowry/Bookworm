using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Recommendations
{
    public class RecommendationsViewModel
    {
        public List<Book> BooksRecommended { get; set; }
        public List<Book> BooksByUsersFavouriteAuthors { get; set; }
        public int UserId { get; set; }
    }
}