using Microsoft.EntityFrameworkCore;
using MonolithServer.Models;

namespace MonolithServer.Database
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Stock>? Stocks { get; set; }
        public DbSet<Image>? Images { get; set; }
        public DbSet<StockRating>? StockRatings { get; set; }
        public DbSet<Store>? Stores { get; set; }
        public DbSet<Profile>? Profiles { get; set; }
        public DbSet<Order>? Orders { get; set; }
    }
}
