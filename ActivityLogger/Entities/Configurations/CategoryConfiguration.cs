using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActivityLogger.Entities.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Work"
                },
                new Category
                {
                    Id = 2,
                    Name = "Project A",
                    ParentId = 1
                },
                new Category
                {
                    Id = 3,
                    Name = "Project B",
                    ParentId = 1
                },
                new Category
                {
                    Id = 4,
                    Name = "Relaxation"
                },
                new Category
                {
                    Id = 5,
                    Name = "Meal"
                },
                new Category
                {
                    Id = 6,
                    Name = "Breakfast",
                    ParentId = 5
                },
                new Category
                {
                    Id = 7,
                    Name = "Lunch",
                    ParentId = 5
                },
                new Category
                {
                    Id = 8,
                    Name = "Dinner",
                    ParentId = 5
                }
            );
        }
    }
}