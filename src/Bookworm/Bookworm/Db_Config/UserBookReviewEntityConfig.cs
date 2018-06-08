using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Bookworm.Db_Config
{
    public class UserBookReviewEntityConfig : EntityTypeConfiguration<UserBookReview>
    {
        public UserBookReviewEntityConfig()
        {
            ToTable("UserBookReviews");

            Property(u => u.UserId).IsRequired();
            Property(u => u.BookId).IsRequired();
            Property(u => u.ReviewId).IsRequired();
            Property(u => u.Rating).IsRequired();
        }
    }
}