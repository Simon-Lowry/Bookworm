using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookworm.Models;
using Bookworm.ViewModels.Profiles;

namespace Bookworm.Contracts.Services
{
    public interface IProfileService
    {
        bool AddConnection(Connection conection);
        bool DeleteConnection(Connection connection);
        bool AreUsersConnected(Connection connection);
        User GetUserDetails(int userId);
        MyConnectionDetails GetAllOfAUsersConnectionsDetails(int userId);
        bool DeleteUserAccount(User user);
    }
}
