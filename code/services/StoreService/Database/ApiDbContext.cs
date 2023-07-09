using Microsoft.EntityFrameworkCore;
using StoreService.Models;

namespace StoreService.Database
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Stores> Stores { get; set; }
    }
}
