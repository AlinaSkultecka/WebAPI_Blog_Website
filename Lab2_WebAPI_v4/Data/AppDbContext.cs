using Lab2_WebAPI_v4.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab2_WebAPI_v4.Data
{
    public class AppDbContext : DbContext
    {
        //Här sätts options som skickas med när contexten sätts upp med DI
        //Exempel på detta är connectionsträngen
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        //Detta blir en tabell i databasen
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Fitness" },
                new Category { CategoryID = 2, CategoryName = "Fashion" },
                new Category { CategoryID = 3, CategoryName = "Health" },
                new Category { CategoryID = 4, CategoryName = "Technology" },
                new Category { CategoryID = 5, CategoryName = "Travel" },
                new Category { CategoryID = 6, CategoryName = "Food" },
                new Category { CategoryID = 7, CategoryName = "Education" },
                new Category { CategoryID = 8, CategoryName = "Business" },
                new Category { CategoryID = 9, CategoryName = "Lifestyle" },
                new Category { CategoryID = 10, CategoryName = "Science" }
            );

            // FIX: break multiple cascade paths
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
