using SabthokFoodWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SabthokFoodWeb.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ItemType> CoverTypes { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<ApplicationUsers> ApplicationUser { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
