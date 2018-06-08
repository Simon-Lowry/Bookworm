using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Search;

namespace Bookworm.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;


        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }


   //     [HttpPost]   
        public ActionResult Search(string searchQuery)
        {
            // search for users
            List<User> users = _searchService.SearchForUsers(searchQuery);

            // search for books
            List<Book> books = _searchService.SearchForBooks(searchQuery);

            // add to search results view model
            SearchResultsViewModel searchResults = new SearchResultsViewModel();
            searchResults.Books = books;
            searchResults.Users = users;

            ViewBag.Message = "Search View";
            return View(searchResults);
        }
    }
}