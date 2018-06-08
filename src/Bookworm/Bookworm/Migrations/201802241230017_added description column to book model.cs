namespace Bookworm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddescriptioncolumntobookmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Isbn", c => c.String());
            AddColumn("dbo.Books", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Description");
            DropColumn("dbo.Books", "Isbn");
        }
    }
}
