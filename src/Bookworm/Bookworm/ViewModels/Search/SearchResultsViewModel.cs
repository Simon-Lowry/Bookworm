using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Search
{
    public class SearchResultsViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}