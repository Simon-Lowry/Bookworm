using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class KindleStoredText
    {
        public Boolean TextType { get; set; }
	    public string Location { get; set; }
	    public string BookTitle { get; set; }
	    public string Author { get; set; }
	    public string TimeDateAddedOn { get; set; }
        public string Text { get; set; }

        [Key]
        public int TextId { get; set; }

        public int UserId { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}