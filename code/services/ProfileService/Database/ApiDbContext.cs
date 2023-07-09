using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Database
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Profiles> Profiles { get; set; }
    }
}
