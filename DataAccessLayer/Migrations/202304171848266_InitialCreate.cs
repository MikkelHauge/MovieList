namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movie",
                c => new
                {
                    MovieId = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                })
                .PrimaryKey(t => t.MovieId);

            CreateTable(
                "dbo.User",
                c => new
                {
                    UserId = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.UserId);

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
            DropTable("dbo.UserMovie");
            DropTable("dbo.User");
            DropTable("dbo.Movie");
        }
    }
}
