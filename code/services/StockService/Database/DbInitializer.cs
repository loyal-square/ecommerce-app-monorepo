using StockService.Models;

namespace StockService.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (!ctx.Stocks.Any())
            {
                ctx.Stocks.Add(new Stocks { Summary = "plumbus" });
                ctx.Stocks.Add(new Stocks { Summary = "flux capacitor" });
                ctx.Stocks.Add(new Stocks { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }
        }
    }
}
