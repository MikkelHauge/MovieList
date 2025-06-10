namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Movie", newName: "Movies");
            RenameTable(name: "dbo.User", newName: "Users");
            RenameTable(name: "dbo.UserMovie", newName: "UserMovies");
            DropIndex("dbo.Movies", new[] { "ReleaseYear", "Title" });
            AlterColumn("dbo.Movies", "Title", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Movies", new[] { "ReleaseYear", "Title" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Movies", new[] { "ReleaseYear", "Title" });
            AlterColumn("dbo.Movies", "Title", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Movies", new[] { "ReleaseYear", "Title" }, unique: true);
            RenameTable(name: "dbo.UserMovies", newName: "UserMovie");
            RenameTable(name: "dbo.Users", newName: "User");
            RenameTable(name: "dbo.Movies", newName: "Movie");
        }
    }
}
