using Microsoft.EntityFrameworkCore;
using StockService.Models;

namespace StockService.Database
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Stocks> Stocks { get; set; }
    }
}
