using Microsoft.EntityFrameworkCore;

namespace MonolithServer.Database
{
    public static class DbInitializer
    {
        public static ApiDbContext context { get; set; }
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.Migrate();
            context = ctx;
        }
    }
}
