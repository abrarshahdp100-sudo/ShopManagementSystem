using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ShopModel> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<CashClosing> CashClosings { get; set; }
    }
}