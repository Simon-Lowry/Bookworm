using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Bookworm.Db_Config
{
    public class KindleStoredTextEntityConfig : EntityTypeConfiguration<KindleStoredText>
    {
        public KindleStoredTextEntityConfig()
        {
            ToTable("KindleStoredText");

            Property(k => k.UserId).IsRequired();
            Property(k => k.TextId).IsRequired();
            Property(k => k.Text).IsRequired();
        }
    }
}
 