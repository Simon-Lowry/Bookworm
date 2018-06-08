using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class Connection
    {
        public int UserId { get; set; }
        public int OtherUserId { get; set; }
        public int ConnectionId { get; set; }

    }
}