using BusinessLogicLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Remoting.Contexts;


namespace DataAccessLayer.Context
{
        public class Context : DbContext
        {
            public Context() : base("MovieList")
            {
                Database.SetInitializer<Context>(new DBinitialiser());
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Movie> Movies { get; set; }

    
        }
    }




