using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ZH.Persistence
{
    public class ZHDbContext : IdentityDbContext
    {
        public ZHDbContext(DbContextOptions<ZHDbContext> options)
            : base(options)
        {
            // EMPTY
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<ArticleRating> ArticleRatings { get; set; }
    }
}
