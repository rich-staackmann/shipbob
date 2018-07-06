using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShipBob.Data.Models
{
    public class UserOrderContext : DbContext
    {
        public UserOrderContext(DbContextOptions<UserOrderContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
