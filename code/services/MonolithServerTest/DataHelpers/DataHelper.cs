using MonolithServer.Models;
using Newtonsoft.Json;

namespace MonolithServerTest.DataHelpers;

public class DataHelper
{

    public List<dynamic> GenerateStocksWithRatingData()
    {
        return new List<dynamic>()
        {
            new StockWithRatingData()
            {
                Id = 1,
                Available = false,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-1).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(3000).Date,
                Name = "Product Name 1",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier(),
                NumberOfRatings = 1,
                AverageRating = 5
            },
            new StockWithRatingData()
            {
                Id = 2,
                Available = false,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-2).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(31).Date,
                Name = "Product Name 2",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier(),
                NumberOfRatings = 1,
                AverageRating = 4
            },
            new StockWithRatingData()
            {
                Id = 3,
                Available = true,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-3).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(32).Date,
                Name = "Product Name 3",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier(),
                NumberOfRatings = 1,
                AverageRating = 3
            },
            new StockWithRatingData()
            {
                Id = 4,
                Available = true,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-4).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(33).Date,
                Name = "Product Name 4",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier(),
                NumberOfRatings = 1,
                AverageRating = 2
            }
        };
    }
    
    public List<StockRating> GenerateStockRatings()
    {
        return  new List<StockRating>
        {
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                UserId = 1,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 4,
                UserId = 1,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1
            },
            new StockRating()
            {
                Id = 3,
                RatingValue = 3,
                UserId = 1,
                RatedDate = DateTime.UtcNow,
                StockId = 3,
                StoreId = 1
            },
            new StockRating()
            {
                Id = 4,
                RatingValue = 2,
                UserId = 1,
                RatedDate = DateTime.UtcNow,
                StockId = 4,
                StoreId = 1
            },
        };
    }
    public List<Stock> GenerateUniqueStocks()
    {

        // Convert the PriceMultiplier object to a JSON string
        
        var stockList = new List<Stock>()
        {
            new Stock()
            {
                Id = 1,
                Available = false,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-1).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(3000).Date,
                Name = "Product Name 1",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier()
            },
            new Stock()
            {
                Id = 2,
                Available = false,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-2).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(31).Date,
                Name = "Product Name 2",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier()
            },
            new Stock()
            {
                Id = 3,
                Available = true,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-3).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(32).Date,
                Name = "Product Name 3",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier()
            },
            new Stock()
            {
                Id = 4,
                Available = true,
                CategoryId = 1,
                CreatedDate = DateTime.UtcNow.AddDays(-4).Date,
                Currency = "NZD",
                Description = "Hello There I'm a description :)",
                Details = "And I'm the details!",
                ExpiryDate = DateTime.UtcNow.AddDays(33).Date,
                Name = "Product Name 4",
                Price = 1234.5f,
                Quantity = 50,
                StoreId = 1,
                PriceMultiplierObjString = GenerateNewPriceMultiplier()
            }
        };
        return stockList;
    }

    public static string GenerateNewPriceMultiplier(float multiplier = 0.5f, int daysBeforeToday = 2, int daysAfterToday = 30)
    {
        var rnd = new Random();
        var priceMultiplier = new PriceMultiplier
        {
            DecimalMultiplier = multiplier,
            CreatedDate = DateTime.Now.AddDays(-daysBeforeToday).Date, // Set to two weeks ago
            ExpiryDate = DateTime.Now.AddMonths(daysAfterToday).Date,  // Set to two months from now
        };
        return JsonConvert.SerializeObject(priceMultiplier);
    }

    public List<StockAndStockWithRatings> GenerateNewStockTestObjects(int amount)
    {
        var objects = new List<StockAndStockWithRatings>();
        for (var i = 0; i < amount; i++)
        {
            objects.Add(new StockAndStockWithRatings());
        }

        return objects;
    }
}

public class StockAndStockWithRatings
{
    private static int _idCounter= 1;
    public readonly StockWithRatingData StockWithRatingData;
    public readonly Stock Stock;
    public readonly int Id;
    public StockAndStockWithRatings()
    {
       StockWithRatingData = new StockWithRatingData
       {
           Id = _idCounter,
           Available = true,
           CategoryId = 1,
           CreatedDate = DateTime.UtcNow.AddDays(-1).Date,
           Currency = "NZD",
           Description = "Hello There I'm a description :)",
           Details = "And I'm the details!",
           ExpiryDate = DateTime.UtcNow.AddDays(30).Date,
           Name = "Product Name 1",
           Price = 1234.5f,
           Quantity = 50,
           StoreId = 1,
           PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(),
       };
       
       Stock = new Stock
       {
           Id = _idCounter,
           Available = true,
           CategoryId = 1,
           CreatedDate = DateTime.UtcNow.AddDays(-1).Date,
           Currency = "NZD",
           Description = "Hello There I'm a description :)",
           Details = "And I'm the details!",
           ExpiryDate = DateTime.UtcNow.AddDays(30).Date,
           Name = "Product Name 1",
           Price = 1234.5f,
           Quantity = 50,
           StoreId = 1,
           PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(),
       };

       Id = _idCounter;

       _idCounter++;
    }
}