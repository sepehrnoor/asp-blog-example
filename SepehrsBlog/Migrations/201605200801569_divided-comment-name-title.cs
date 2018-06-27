namespace SepehrsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dividedcommentnametitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Name", c => c.String());
            DropColumn("dbo.Comments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Name");
        }
    }
}
