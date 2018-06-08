 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImgUrl { get; set; }

        // Navigation Property
        public ICollection<UserBookReview> UserBookReviews { get; set; }
        public ICollection<KindleStoredText> StoredTxt { get; set; }
    }
}