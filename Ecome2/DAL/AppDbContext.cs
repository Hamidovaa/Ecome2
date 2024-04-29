using Ecome2.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.DAL
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}
        public DbSet<Slider>Sliders { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}
