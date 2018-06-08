namespace Bookworm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsolidateRatingsInBookTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BookRatings", "BookId", "dbo.Books");
            DropIndex("dbo.BookRatings", new[] { "BookId" });
            AddColumn("dbo.Books", "AvgRating", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Ratings_1", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Ratings_2", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Ratings_3", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Ratings_4", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Ratings_5", c => c.Double(nullable: false));
            DropTable("dbo.BookRatings");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.BookId);
            
            DropColumn("dbo.Books", "Ratings_5");
            DropColumn("dbo.Books", "Ratings_4");
            DropColumn("dbo.Books", "Ratings_3");
            DropColumn("dbo.Books", "Ratings_2");
            DropColumn("dbo.Books", "Ratings_1");
            DropColumn("dbo.Books", "AvgRating");
            CreateIndex("dbo.BookRatings", "BookId");
            AddForeignKey("dbo.BookRatings", "BookId", "dbo.Books", "BookId");
        }
    }
}
