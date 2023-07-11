using StockService.Models;

namespace StockService.Database
{
    public static class DbInitializer
    {
        public static ApiDbContext context { get; set; }
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            context = ctx;
        }
    }
}
