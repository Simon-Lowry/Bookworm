namespace Bookworm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedToReadIdAsKeyForToReadsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "ToRead_UserId", "dbo.ToReads");
            DropForeignKey("dbo.ToReads", "UserId", "dbo.Users");
            DropIndex("dbo.Books", new[] { "ToRead_UserId" });
            DropIndex("dbo.ToReads", new[] { "UserId" });
            DropPrimaryKey("dbo.ToReads");
            AddColumn("dbo.ToReads", "ToReadId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ToReads", "ToReadId");
            DropColumn("dbo.Books", "ToRead_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "ToRead_UserId", c => c.Int());
            DropPrimaryKey("dbo.ToReads");
            DropColumn("dbo.ToReads", "ToReadId");
            AddPrimaryKey("dbo.ToReads", "UserId");
            CreateIndex("dbo.ToReads", "UserId");
            CreateIndex("dbo.Books", "ToRead_UserId");
            AddForeignKey("dbo.ToReads", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Books", "ToRead_UserId", "dbo.ToReads", "UserId");
        }
    }
}
