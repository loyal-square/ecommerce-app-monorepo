using StoreService.Models;

namespace StoreService.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (!ctx.Stores.Any())
            {
                ctx.Stores.Add(new Stores { Summary = "plumbus" });
                ctx.Stores.Add(new Stores { Summary = "flux capacitor" });
                ctx.Stores.Add(new Stores { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }
        }
    }
}
