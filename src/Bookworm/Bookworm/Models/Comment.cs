using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class Comment
    {
        public string CommentText { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
    }
}