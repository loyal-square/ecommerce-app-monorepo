using ProfileService.Models;

namespace ProfileService.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (!ctx.Profiles.Any())
            {
                ctx.Profiles.Add(new Profiles { Summary = "plumbus" });
                ctx.Profiles.Add(new Profiles { Summary = "flux capacitor" });
                ctx.Profiles.Add(new Profiles { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }
        }
    }
}
