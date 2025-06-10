namespace DataAccessLayer.Migrations
{
    using BusinessLogicLayer.Model;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.Context.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DataAccessLayer.Context.Context";
        }

        
    }
}
