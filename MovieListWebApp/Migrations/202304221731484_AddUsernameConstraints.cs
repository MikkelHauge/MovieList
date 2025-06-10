namespace MovieListWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsernameConstraints : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        ReleaseYear = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.MovieId)
                .Index(t => new { t.ReleaseYear, t.Title }, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UserMovies",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Movie_MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Movie_MovieId })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Movie_MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMovies", "Movie_MovieId", "dbo.Movies");
            DropForeignKey("dbo.UserMovies", "User_UserId", "dbo.Users");
            DropIndex("dbo.UserMovies", new[] { "Movie_MovieId" });
            DropIndex("dbo.UserMovies", new[] { "User_UserId" });
            DropIndex("dbo.Users", new[] { "Name" });
            DropIndex("dbo.Movies", new[] { "ReleaseYear", "Title" });
            DropTable("dbo.UserMovies");
            DropTable("dbo.Users");
            DropTable("dbo.Movies");
        }
    }
}
