using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bookworm.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Authors { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Isbn { get; set; }
        public int PublishedYear { get; set; }
        public string Description { get; set; }

        public string BookImgUrl { get; set; }
        //  public int Pages(missing from thingy)

        public double AvgRating { get; set; }
        public double Ratings_1 { get; set; }
        public double Ratings_2 { get; set; }
        public double Ratings_3 { get; set; }
        public double Ratings_4 { get; set; }
        public double Ratings_5 { get; set; }

    }
}