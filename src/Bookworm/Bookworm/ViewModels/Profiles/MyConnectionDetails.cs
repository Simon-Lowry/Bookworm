using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.ViewModels.Profiles
{
    public class MyConnectionDetails
    {
        public List<Connection> Connections { get; set; }
        public List<User> ConnectionsProfileDetails { get; set; }
    }
}