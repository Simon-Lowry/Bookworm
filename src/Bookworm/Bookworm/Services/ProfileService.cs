using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Profiles;

namespace Bookworm.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Connection> _connectionRepository;


        public ProfileService(IRepository<User> repository, IRepository<Connection> conRepo )
        {
            _userRepository = repository;
            _connectionRepository = conRepo;
        }


        // creates a bi-directional connection between the two users
        // needs to be checked, 
        public bool AddConnection(Connection connection)
        {
            bool success = _connectionRepository.Create(connection);

            if (success == false)
                return false;

            var temp = connection.UserId;
            connection.UserId = connection.OtherUserId;
            connection.OtherUserId = temp;

            success = _connectionRepository.Create(connection);

            return success;
        }


        // needs to be checked, may need to find the second connection first, since i'm not getting it's connection id
        public bool DeleteConnection(Connection connection)
        {
            bool success = _connectionRepository.Delete(connection);

            if (success == false) return false;

            var temp = connection.UserId;
            var tempConId = connection.ConnectionId;
            connection.UserId = connection.OtherUserId;
            connection.ConnectionId = tempConId - 1;
            connection.OtherUserId = temp;

            success = _connectionRepository.Delete(connection);

            return success;
        }


        // needs to be checked,
        public bool AreUsersConnected(Connection connection)
        {
            var userConnections = from connections in _connectionRepository.GetListOf()
                where (connections.UserId == connection.UserId  && connections.OtherUserId == connection.OtherUserId) 
                      || (connections.UserId == connection.OtherUserId && connections.OtherUserId == connection.UserId)
                select connections;

            return userConnections.ToList().Count == 2;
        }
        
        
        // done
        public User GetUserDetails(int userId)
        {
            return _userRepository.Get(userId);
        }


        public bool DeleteUserAccount(User user)
        {
            if (GetUserDetails(user.UserId) == null)
                return false;

            return _userRepository.Delete(user);
        }


        public MyConnectionDetails GetAllOfAUsersConnectionsDetails(int userId)
        {
            List<Connection> myConnections  = (from connections in _connectionRepository.GetListOf()
                                            where (connections.UserId == userId)
                                            select connections).ToList();

            List<User> myConnectionsProfiles = new List<User>();
            for (int i = 0; i < myConnections.Count; i++)
            {
                List<User> connectionProfile = (from users in _userRepository.GetListOf()
                    where (users.UserId == myConnections[i].OtherUserId)
                    select users).ToList();
                if (connectionProfile.Count != 0)
                {
                    myConnectionsProfiles.Add(connectionProfile[0]);
                }
            }

            MyConnectionDetails myConnectionDetails = new MyConnectionDetails()
            {
                Connections = myConnections,
                ConnectionsProfileDetails = myConnectionsProfiles
            };

            return myConnectionDetails;
        }
        
    }
}