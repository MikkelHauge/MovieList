namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addedMovieDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "Description", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Movie", "Description");
        }
    }
}
