using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class ToRead
    {
        public int UserId { get; set; }
        public int BookId { get; set; }

        [Key]
        public int ToReadId { get; set; }

    }
}