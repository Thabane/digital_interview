using System;
using System.Data.Entity;
using System.Linq;
using DataModelCommon;

namespace DataClientLayer
{

    /// <summary>
    /// Inherit from DbContext to use db connection functionality
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class DigitalInterviewDbContext : DbContext
    {
        // Your context has been configured to use a 'DigitalInterviewDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DataClientLayer.DigitalInterviewDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DigitalInterviewDbContext' 
        // connection string in the application configuration file.
        public DigitalInterviewDbContext()
            : base("name=DigitalInterviewDbContext")
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Subcription> subcriptions { get; set; }
        public DbSet<UserSubcription> userSubcriptions { get; set; }
        public DbSet<Credit> credit { get; set; }
        public DbSet<Voucher> couchers { get; set; }
        public DbSet<Resource> resource { get; set; }
        public DbSet<ToDoList> toDoList { get; set; }


    }
}