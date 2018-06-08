using Bookworm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookworm.Contracts
{
    public interface IBookwormDbContext 
    {
        DbSet<Book> Books { get; set; }
        DbSet<KindleStoredText> KindleStoredText { get; set; }
        DbSet<ToRead> ToRead { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserBookReview> UserRatingOfBooks { get; set; }
        //    public DbSet<Comment> Comments { get; set; }
        

        /// <summary>
        ///     Method used to persist changes
        /// </summary>        
        int SaveChanges();
       
        /// <summary>
        ///     Method used to persist changes
        /// </summary>        
        Task<int> SaveChangesAsync();
        
    }
}
