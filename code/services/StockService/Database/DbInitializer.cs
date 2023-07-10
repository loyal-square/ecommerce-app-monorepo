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

            if (!ctx.Stocks.Any())
            {
                ctx.Stocks.Add(new Stocks { Summary = "plumbus" });
                ctx.Stocks.Add(new Stocks { Summary = "flux capacitor" });
                ctx.Stocks.Add(new Stocks { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }

            if (!ctx.Images.Any())
            {   
                ctx.Images.Add(new Images { AWSImgUrl = "https://loyalsquare-ecommerce-images.s3.ap-southeast-2.amazonaws.com/asuna.png" });
                ctx.Images.Add(new Images { AWSImgUrl = "https://loyalsquare-ecommerce-images.s3.ap-southeast-2.amazonaws.com/asuna.png" });
                ctx.Images.Add(new Images { AWSImgUrl = "https://loyalsquare-ecommerce-images.s3.ap-southeast-2.amazonaws.com/asuna.png" });
                ctx.SaveChanges();
            }
        }
    }
}
