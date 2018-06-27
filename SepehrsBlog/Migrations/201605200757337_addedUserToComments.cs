namespace SepehrsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserToComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Title", c => c.String());
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Name", c => c.String());
            DropColumn("dbo.Comments", "UserId");
            DropColumn("dbo.Comments", "Title");
        }
    }
}
