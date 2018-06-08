using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Profiles
{
    public class CombinedUserProfilesViewModel
    {
        public User MyUserDetails { get; set; } 
        public User OtherUserDetails { get; set; }
        public bool UsersAreConnected { get; set; }
        public Connection Connection { get; set; }

        public int ConnectionWasCreated { get; set; }
        public int ConnectionWasDeleted { get; set; }

        public MyBookReviewsDetails OtherUsersBookReviews { get; set; }
        public MyConnectionDetails OtherUsersConnections { get; set; }
        public MyToReadDetails OtherUsersToReadBookDetails { get; set; }

        public const int Success = 1;
        public const int Failed = 2;
        public const int NotSet = 0;
    }
}