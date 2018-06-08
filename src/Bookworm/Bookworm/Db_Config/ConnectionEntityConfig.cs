using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace Bookworm.Db_Config
{
    public class ConnectionEntityConfig : EntityTypeConfiguration<Connection>
    {
        public ConnectionEntityConfig()
        {
            ToTable("Connections");

            Property(c => c.UserId).IsRequired();
            Property(c => c.OtherUserId).IsRequired();
        }
    }
}