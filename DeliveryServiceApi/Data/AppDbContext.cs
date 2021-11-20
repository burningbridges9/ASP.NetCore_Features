using Microsoft.EntityFrameworkCore;

namespace DeliveryServiceApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {

        }
    }
}
