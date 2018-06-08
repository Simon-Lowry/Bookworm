using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookworm.Models;
using NReco.CF.Taste.Impl.Model;
using NReco.CF.Taste.Recommender;

namespace Bookworm.Contracts.Services
{
    public interface IRecommenderService
    {
        List<int> GetRecommendations(int userId);
        List<Book> GetBooksRecommended(List<int> bookId);
        long[] GetNNearestNeighborsUsersRecommendations(int numNeighbours, int userId);
        GenericDataModel GetUserBasedDataModel();
        List<Book> GetBooksFromAuthorsAUserLiked(int userId);

    }
}
