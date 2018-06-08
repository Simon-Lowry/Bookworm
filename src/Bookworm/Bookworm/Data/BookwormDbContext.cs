using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Bookworm.Contracts;
using Bookworm.Models;
using System.Threading.Tasks;

namespace Bookworm.Data
{
    public class BookwormDbContext : DbContext, IBookwormDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<KindleStoredText> KindleStoredText { get; set; }
        public DbSet<ToRead> ToRead { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBookReview>UserRatingOfBooks { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public BookwormDbContext() : base("IBookwormDbContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<BookwormDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        ///     Method used to persist changes
        /// </summary>        
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        ///     Method used to persist changes
        /// </summary>        
        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
           //     _logger.Error(ex, $"Error saving changes: {ex.Message}");
                throw;
            }
        }
    }
}