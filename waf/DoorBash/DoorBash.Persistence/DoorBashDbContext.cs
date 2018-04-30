using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoorBash.Persistence
{
    public class DoorBashDbContext : IdentityDbContext<User>
    {
        public DoorBashDbContext(DbContextOptions<DoorBashDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }
    }
}
