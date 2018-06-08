using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Bookworm.Db_Config
{
    public class BookEntityConfig : EntityTypeConfiguration<Book>
    {
        public BookEntityConfig()
        {
            ToTable("Books");

            Property(b => b.BookId).IsRequired();
            Property(b => b.Authors).IsRequired();
            Property(b => b.Title).IsRequired(); 
        }
    }
}