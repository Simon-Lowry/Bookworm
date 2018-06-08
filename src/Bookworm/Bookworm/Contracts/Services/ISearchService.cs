using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookworm.Models;

namespace Bookworm.Contracts.Services
{
    public interface ISearchService
    {
        List<User> SearchForUsers(string searchQuery);
        List<Book> SearchForBooks(string searchQuery);
    }
}
