using Ecome2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.DAL
{
    public class AppDbContext :IdentityDbContext<ProgramUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}
        public DbSet<Slider>Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Images> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasData(
                new Order {Id=1, ProductId=1, UserId=2},
                new Order { Id = 2, ProductId = 2, UserId = 1 }
                );

        }
    }
}
