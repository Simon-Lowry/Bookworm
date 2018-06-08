using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Search;
using Microsoft.Ajax.Utilities;

namespace Bookworm.Services
{
    public class SearchService : ISearchService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Book> _booksRepository;

        public SearchService(IRepository<User> users, IRepository<Book> books)
        {
            _userRepository = users;
            _booksRepository = books;
        }


        public List<User> SearchForUsers(string searchQuery)
        {          
            int indexOfSpace = searchQuery.IndexOf(' ');
  
            if (indexOfSpace != -1)             // if there is a space, we're searching for a user  since all users must have two names
            {
                string firstName = searchQuery.Substring(0, indexOfSpace);
                string lastName = searchQuery.Substring(indexOfSpace + 1);

                var userProfiles = from users in _userRepository.GetListOf()
                    where users.FirstName.Contains(firstName) || users.LastName.Contains(lastName)
                    select users;

               return userProfiles.ToList();
            }

            return null;
        }

        public List<Book> SearchForBooks(string searchQuery)
        {
            var booksProfiles = from books in _booksRepository.GetListOf() where (books.Title.Contains(searchQuery) ) select books;

            return booksProfiles.ToList();
        }
    }
}