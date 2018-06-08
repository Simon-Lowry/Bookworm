using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Profiles
{
    public class MyToReadDetails
    {
        public List<ToRead> ToReadShelf { get; set; }
        public List<Book> ToReadBooksDetails { get; set; }
    }
}