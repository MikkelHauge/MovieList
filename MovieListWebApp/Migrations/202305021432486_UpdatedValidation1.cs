namespace MovieListWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedValidation1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Movies", newName: "Movie");
            RenameTable(name: "dbo.Users", newName: "User");
            RenameTable(name: "dbo.UserMovies", newName: "UserMovie");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserMovie", newName: "UserMovies");
            RenameTable(name: "dbo.User", newName: "Users");
            RenameTable(name: "dbo.Movie", newName: "Movies");
        }
    }
}
