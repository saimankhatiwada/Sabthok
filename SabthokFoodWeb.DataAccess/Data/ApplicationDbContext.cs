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
    }
}
