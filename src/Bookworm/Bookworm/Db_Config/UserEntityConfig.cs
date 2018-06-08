using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Bookworm.Db_Config
{
    public class UserEntityConfig : EntityTypeConfiguration<User>
    {
        public UserEntityConfig()
        {
            ToTable("Users");

            HasMany<UserBookReview>(b => b.UserBookReviews)
                .WithMany()
                .Map(
                    m =>
                    {
                        m.MapLeftKey("UserId");
                        m.MapRightKey("UserId");
                    }
             );

            HasMany<KindleStoredText>(b => b.StoredTxt)
                .WithMany()
                .Map(
                     m =>
                     {
                        m.MapLeftKey("UserId");
                        m.MapRightKey("TextId");
                     }
             );
        }
    }
}