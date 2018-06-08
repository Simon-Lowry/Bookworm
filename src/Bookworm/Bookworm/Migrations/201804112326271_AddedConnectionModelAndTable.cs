namespace Bookworm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConnectionModelAndTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        OtherUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Connections");
        }
    }
}
