namespace MovieList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movieTitleLEngth100 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        ReleaseYear = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.MovieId)
                .Index(t => new { t.ReleaseYear, t.Title }, unique: true);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UserMovie",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Movie_MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Movie_MovieId })
                .ForeignKey("dbo.User", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Movie", t => t.Movie_MovieId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Movie_MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMovie", "Movie_MovieId", "dbo.Movie");
            DropForeignKey("dbo.UserMovie", "User_UserId", "dbo.User");
            DropIndex("dbo.UserMovie", new[] { "Movie_MovieId" });
            DropIndex("dbo.UserMovie", new[] { "User_UserId" });
            DropIndex("dbo.User", new[] { "Name" });
            DropIndex("dbo.Movie", new[] { "ReleaseYear", "Title" });
            DropTable("dbo.UserMovie");
            DropTable("dbo.User");
            DropTable("dbo.Movie");
        }
    }
}
