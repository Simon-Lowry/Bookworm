using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Bookworm.Db_Config
{
    public class ToReadEntityConfig : EntityTypeConfiguration<ToRead>
    {
        public ToReadEntityConfig()
        {
            ToTable("ToRead");

            Property(u => u.UserId).IsRequired();
            Property(r => r.BookId).IsRequired();
        }
    }
}