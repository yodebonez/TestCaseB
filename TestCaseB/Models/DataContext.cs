using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TestCaseB.Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
         : base(options)
        {
            //add custom tables 

        }


        public DbSet<ShippingUpdate> shipingUpdates { get; set; }
        public DbSet<Shipping> shippings { get; set; }
        public DbSet<User> users { get; set; }
    }
}
