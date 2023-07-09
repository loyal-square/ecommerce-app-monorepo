using Microsoft.EntityFrameworkCore;
using StoreAnalyticsService.Models;

namespace StoreAnalyticsService.Database
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<StoreAnalytics> StoreAnlytics { get; set; }
    }
}
