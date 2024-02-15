using Lab10.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab10.Data
{
    public class StoreDbContext : IdentityDbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasMany(category => category.Articles)
                .WithOne(article => article.Category)
                .HasForeignKey(article => article.CategoryId);
            //modelBuilder.Seed();
        }

    }
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Food" },
                new Category { CategoryId = 2, Name = "Electronics" },
                new Category { CategoryId = 3, Name = "Cosmetics" },
                new Category { CategoryId = 4, Name = "Home appliances" }
            );
            modelBuilder.Entity<Article>().HasData(
                new Article { ArticleId = 1, Name = "Prince Polo", Price = 1.99M, CategoryId = 1, ImagePath = "~/images/img1.jpg" }
            );
        }
    }
}