﻿using ProfileService.Models;

namespace ProfileService.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (!ctx.Orders.Any())
            {
                ctx.Orders.Add(new Orders { Summary = "plumbus" });
                ctx.Orders.Add(new Orders { Summary = "flux capacitor" });
                ctx.Orders.Add(new Orders { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }
        }
    }
}