namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class uniqueNameSet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "ReleaseYear", c => c.Int(nullable: false));
            AlterColumn("dbo.Movie", "Title", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AlterColumn("dbo.User", "Name", c => c.String(nullable: false, maxLength: 60));
            CreateIndex("dbo.Movie", new[] { "ReleaseYear", "Title" }, unique: true);
            CreateIndex("dbo.User", "Name", unique: true);

        }

        public override void Down()
        {
            DropIndex("dbo.User", new[] { "Name" });
            DropIndex("dbo.Movie", new[] { "ReleaseYear", "Title" });
            AlterColumn("dbo.User", "Name", c => c.String());
            DropColumn("dbo.Movie", "ReleaseYear");
        }
    }
}
