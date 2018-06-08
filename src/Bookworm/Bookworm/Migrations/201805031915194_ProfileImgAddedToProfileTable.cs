namespace Bookworm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileImgAddedToProfileTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProfileImgUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ProfileImgUrl");
        }
    }
}
