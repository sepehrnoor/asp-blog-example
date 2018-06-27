namespace SepehrsBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posts", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Date");
            DropColumn("dbo.Comments", "Date");
        }
    }
}
