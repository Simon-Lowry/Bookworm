using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class UserBookReview
    {
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        [Key]
        public int ReviewId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
       
    }
}
 