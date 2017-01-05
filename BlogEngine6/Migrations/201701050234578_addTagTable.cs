namespace BlogEngine6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTagTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.BlogTags",
                c => new
                    {
                        Tag_TagID = c.Int(nullable: false),
                        Blog_BlogID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagID, t.Blog_BlogID })
                .ForeignKey("dbo.Tags", t => t.Tag_TagID, cascadeDelete: true)
                .ForeignKey("dbo.Blogs", t => t.Blog_BlogID, cascadeDelete: true)
                .Index(t => t.Tag_TagID)
                .Index(t => t.Blog_BlogID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogTags", "Blog_BlogID", "dbo.Blogs");
            DropForeignKey("dbo.BlogTags", "Tag_TagID", "dbo.Tags");
            DropIndex("dbo.BlogTags", new[] { "Blog_BlogID" });
            DropIndex("dbo.BlogTags", new[] { "Tag_TagID" });
            DropTable("dbo.BlogTags");
            DropTable("dbo.Tags");
        }
    }
}
