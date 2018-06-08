namespace Bookworm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Firstmigrationsetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookRatings",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        AvgRating = c.Double(nullable: false),
                        Ratings_1 = c.Double(nullable: false),
                        Ratings_2 = c.Double(nullable: false),
                        Ratings_3 = c.Double(nullable: false),
                        Ratings_4 = c.Double(nullable: false),
                        Ratings_5 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Books", t => t.BookId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Authors = c.String(),
                        Title = c.String(),
                        Language = c.String(),
                        BookImgUrl = c.String(),
                        PublishedYear = c.Int(nullable: false),
                        ToRead_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.ToReads", t => t.ToRead_UserId)
                .Index(t => t.ToRead_UserId);
            
            CreateTable(
                "dbo.KindleStoredTexts",
                c => new
                    {
                        TextId = c.Int(nullable: false, identity: true),
                        TextType = c.Boolean(nullable: false),
                        Location = c.String(),
                        BookTitle = c.String(),
                        Author = c.String(),
                        TimeDateAddedOn = c.String(),
                        Text = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ToReads",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserBookReviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBookReviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.ToReads", "UserId", "dbo.Users");
            DropForeignKey("dbo.Books", "ToRead_UserId", "dbo.ToReads");
            DropForeignKey("dbo.KindleStoredTexts", "UserId", "dbo.Users");
            DropForeignKey("dbo.BookRatings", "BookId", "dbo.Books");
            DropIndex("dbo.UserBookReviews", new[] { "UserId" });
            DropIndex("dbo.ToReads", new[] { "UserId" });
            DropIndex("dbo.KindleStoredTexts", new[] { "UserId" });
            DropIndex("dbo.Books", new[] { "ToRead_UserId" });
            DropIndex("dbo.BookRatings", new[] { "BookId" });
            DropTable("dbo.UserBookReviews");
            DropTable("dbo.ToReads");
            DropTable("dbo.Users");
            DropTable("dbo.KindleStoredTexts");
            DropTable("dbo.Books");
            DropTable("dbo.BookRatings");
        }
    }
}
