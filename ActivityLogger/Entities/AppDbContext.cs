using ActivityLogger.Entities.Configurations;
using ActivityLogger.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityLogger.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}