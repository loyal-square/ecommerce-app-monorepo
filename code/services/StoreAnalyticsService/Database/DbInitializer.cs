using StoreAnalyticsService.Models;

namespace StoreAnalyticsService.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (!ctx.StoreAnlytics.Any())
            {
                ctx.StoreAnlytics.Add(new StoreAnalytics { Summary = "plumbus" });
                ctx.StoreAnlytics.Add(new StoreAnalytics { Summary = "flux capacitor" });
                ctx.StoreAnlytics.Add(new StoreAnalytics { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }
        }
    }
}
