namespace FunApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 50),
                        Price = c.Double(nullable: false),
                        PublisherID = c.Int(),
                        CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Publishers", t => t.PublisherID)
                .Index(t => t.PublisherID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "PublisherID", "dbo.Publishers");
            DropForeignKey("dbo.Games", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Games", new[] { "CategoryID" });
            DropIndex("dbo.Games", new[] { "PublisherID" });
            DropTable("dbo.Publishers");
            DropTable("dbo.Games");
            DropTable("dbo.Categories");
        }
    }
}
