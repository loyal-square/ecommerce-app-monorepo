using System.Globalization;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonolithServer;
using MonolithServer.Controllers;
using MonolithServer.Database;
using MonolithServer.Managers.Implementations;
using MonolithServer.Models;
using MonolithServerTest.DataHelpers;
using Newtonsoft.Json;

namespace MonolithServerTest.Managers;

public class StockManagerTest
{
    
    private readonly ApiDbContext _dataContext;
    private readonly IMapper _mapper;

    public StockManagerTest()
    {
        DbContextOptions<ApiDbContext> dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        _dataContext = new ApiDbContext(dbContextOptions);
        
        MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(MappingProfile)));
        _mapper = new Mapper(configuration);
    }
    #region GetAllStocksWithFilters

    [Theory]
    [InlineData(1, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(35, 10, 4)]
    [InlineData(0, 10, 0)]
    public async void GetAllStocksWithFilters_NoFilters_XPages_ShouldReturnAll(int amountOfStock, int itemsPerPage , int expectedTotalPages)
    {
        //Arrange 
        var dataHelper = new DataHelper();
        var stockAndStockWithRatingsList = dataHelper.GenerateNewStockTestObjects(amountOfStock);
        var stockList = stockAndStockWithRatingsList.Select(x => x.Stock).ToList();
        var stockWithRatingList = stockAndStockWithRatingsList.Select(x => x.StockWithRatingData).ToList();

        var paginatedItems = new List<StockWithRatingData>();

        if (amountOfStock < itemsPerPage)
        {
            paginatedItems.AddRange(stockWithRatingList);
        }
        else
        {
            for (var i = 0; i < itemsPerPage; i++)
            {
                paginatedItems.Add(stockWithRatingList[i]);
            }
        }
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = paginatedItems,
            TotalItems = amountOfStock,
            TotalPages = expectedTotalPages
        };

        var idsStock = stockList.Select(x => x.Id);
        var idsRatings = stockWithRatingList.Select(x => x.Id);
        
        await _dataContext.Stocks.AddRangeAsync(stockList);
        await _dataContext.SaveChangesAsync();

        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);

        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(null, null,null,null,null,1,itemsPerPage);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(paginatedResult), JsonConvert.SerializeObject(result));
    }
    
    [Theory]
    [InlineData(true, null ,null, 4)]
    [InlineData(null, true ,null, 4)]
    [InlineData(null, null ,true, 4)]
    [InlineData(true, true ,true, 4)]
    [InlineData(true, true ,null, 4)]
    public async void GetAllStocksWithFilters_ShouldReturnAll(bool? ascendingNames, bool? ascendingPrices, bool? ascendingCreatedDates, int expectedTotalStockCount)
    {
        //Arrange 
        var dataHelper = new DataHelper();
        var stockList = dataHelper.GenerateUniqueStocks();
        var ratingList = dataHelper.GenerateStockRatings();
        var stockWithRatingList = dataHelper.GenerateStocksWithRatingData();
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = stockWithRatingList,
            TotalItems = expectedTotalStockCount,
            TotalPages = 1
        };
        
        await _dataContext.Stocks.AddRangeAsync(stockList);
        await _dataContext.StockRatings.AddRangeAsync(ratingList);
        await _dataContext.SaveChangesAsync();

        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);

        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(ascendingNames, ascendingPrices, ascendingCreatedDates, null, null, 1, 10);
        
        //Assert
        Assert.Equal(paginatedResult.TotalItems, result.TotalItems);
        Assert.Equal(paginatedResult.PaginatedItems.Count, result.PaginatedItems.Count);
    }
    
    [Theory]
    [InlineData(5, 4, 1)]
    [InlineData(100, 0, 0)]
    [InlineData(50, 4, 1)]
    public async void GetAllStocksWithFilters_MinimumQuantity_ShouldReturnAll(int minimumQuantity, int expectedTotalItems, int expectedTotalPages)
    {
        //Arrange 
        var dataHelper = new DataHelper();
        var stockList = dataHelper.GenerateUniqueStocks();
        var ratingList = dataHelper.GenerateStockRatings();
        var stockWithRatingList = dataHelper.GenerateStocksWithRatingData();
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = stockWithRatingList.FindAll(x => x.Quantity >= minimumQuantity),
            TotalItems = expectedTotalItems,
            TotalPages = expectedTotalPages
        };
        
        await _dataContext.Stocks.AddRangeAsync(stockList);
        await _dataContext.StockRatings.AddRangeAsync(ratingList);
        await _dataContext.SaveChangesAsync();

        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);

        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(null, null,null,null,minimumQuantity,1,10);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(paginatedResult), JsonConvert.SerializeObject(result));
    }

    [Fact]
    public async void GetAllStocksWithFilters_AscendingNames_ShouldBeInCorrectOrder()
    {
        //Arrange
        var unOrderedStockNames = new List<Stock>()
        {
            new Stock()
            {
                Id = 1,
                Name = "Z",
                CreatedDate = DateTime.UtcNow.Date
            },
            new Stock()
            {
                Id = 2,
                Name = "A",
                CreatedDate = DateTime.UtcNow.Date
            }
        };
        
        var orderedStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 2,
                Name = "A",
                CreatedDate = DateTime.UtcNow.Date
            },
            new StockWithRatingData()
            {
                Id = 1,
                Name = "Z",
                CreatedDate = DateTime.UtcNow.Date
            }
        };
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = orderedStock,
            TotalItems = 2,
            TotalPages = 1
        };
        
        await _dataContext.Stocks.AddRangeAsync(unOrderedStockNames);
        await _dataContext.SaveChangesAsync();
        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);
        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(true, null,null,null, null,1,10);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(paginatedResult), JsonConvert.SerializeObject(result));

    }
    
    [Fact]
    public async void GetAllStocksWithFilters_AscendingPrices_NoMultipliers_ShouldBeInCorrectOrder()
    {
        //Arrange
        var unOrderedStockNames = new List<Stock>()
        {
            new Stock()
            {
                Id = 1,
                Name = "A",
                Price = 1000,
                CreatedDate = DateTime.UtcNow.Date
            },
            new Stock()
            {
                Id = 2,
                Name = "B",
                Price = 500,
                CreatedDate = DateTime.UtcNow.Date
            }
        };
        
        var orderedStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 2,
                Name = "B",
                Price = 500,
                CreatedDate = DateTime.UtcNow.Date
            },
            new StockWithRatingData()
            {
                Id = 1,
                Name = "A",
                Price = 1000,
                CreatedDate = DateTime.UtcNow.Date
            }
        };
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = orderedStock,
            TotalItems = 2,
            TotalPages = 1
        };
        
        await _dataContext.Stocks.AddRangeAsync(unOrderedStockNames);
        await _dataContext.SaveChangesAsync();
        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);
        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(null, true,null,null, null,1,10);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(paginatedResult), JsonConvert.SerializeObject(result));

    }
    
    [Fact]
    public async void GetAllStocksWithFilters_AscendingPrices_WithMultipliers_ShouldBeInCorrectOrder()
    {
        //Arrange
        var unOrderedStockNames = new List<Stock>()
        {
            new Stock()
            {
                Id = 1,
                Name = "A",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(3),
                CreatedDate =  DateTime.UtcNow.Date
            },
            new Stock()
            {
                Id = 2,
                Name = "B",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(2),
                CreatedDate =  DateTime.UtcNow.Date
            },
            new Stock()
            {
                Id = 3,
                Name = "C",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(1),
                CreatedDate =  DateTime.UtcNow.Date
            }
        };
        
        var orderedStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 3,
                Name = "C",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(1),
                CreatedDate =  DateTime.UtcNow.Date
            },
            new StockWithRatingData()
            {
                Id = 2,
                Name = "B",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(2),
                CreatedDate =  DateTime.UtcNow.Date
            },
             new StockWithRatingData()
             {
                 Id = 1,
                 Name = "A",
                 Price = 1000,
                 PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(3),
                 CreatedDate =  DateTime.UtcNow.Date
             },
        };
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = orderedStock,
            TotalItems = 3,
            TotalPages = 1
        };
        
        await _dataContext.Stocks.AddRangeAsync(unOrderedStockNames);
        await _dataContext.SaveChangesAsync();
        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);
        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(null, true,null,null, null,1,10);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(paginatedResult), JsonConvert.SerializeObject(result));

    }

    [Fact]
    public async void GetAllStocksWithFilters_OnlyAvailable_ShouldReturnCorrectStocks()
    {
        //Arrange
        var unOrderedStockNames = new List<Stock>()
        {
            new Stock()
            {
                Id = 1,
                Available = true,
                Name = "Z",
                CreatedDate =  DateTime.UtcNow.Date
            },
            new Stock()
            {
                Id = 2,
                Available = false,
                Name = "A",
                CreatedDate =  DateTime.UtcNow.Date
            }
        };

        var correctStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 1,
                Name = "Z",
                Available = true,
                CreatedDate =  DateTime.UtcNow.Date
            }
        };
        
        var paginatedResult = new PaginatedResult()
        {
            PaginatedItems = correctStock,
            TotalItems = 1,
            TotalPages = 1
        };
        
        await _dataContext.Stocks.AddRangeAsync(unOrderedStockNames);
        await _dataContext.SaveChangesAsync();
        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);
        
        //Act
        var result = await stockManager.GetAllStocksWithFilters(null, null,null,true, null,1,10);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(paginatedResult), JsonConvert.SerializeObject(result));
    }

    #endregion
    #region GetStocksByStockIds
    
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public async void GetStocksByStockIds_ReturnsCorrectStocks(int amount)
    {
        //Arrange
        var dataHelper = new DataHelper();
        var stocks = dataHelper.GenerateNewStockTestObjects(amount);
        var expectedResult = new PaginatedResult()
        {
            PaginatedItems = stocks.Select(x => x.StockWithRatingData).ToList(),
            TotalItems = amount,
            TotalPages = amount > 0 ? 1 : 0
        };
        
        await _dataContext.Stocks.AddRangeAsync(stocks.Select(x => x.Stock).ToList());
        await _dataContext.SaveChangesAsync();
        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);
        var stockIds = stocks.Select(x => x.Id).ToList();
        //Act
        var result = await stockManager.GetStocksByStockIds(stockIds, 1, 10);
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
    }
    #endregion
    #region GetOnSaleStocks

    [Theory]
    [InlineData(1f, null, 1, 10, 0, 5, 5)]
    [InlineData(0.5f, null, 1, 10, 0, 5, 5)]
    [InlineData(0.5f, "Product Name", 1, 10, 0, 5, 5)]
    [InlineData(1f, "Product Name", 1, 10, 0, 5, 5)]
    [InlineData(0.5f, "Product Name", 1, 10, 4, 5, 0)]
    [InlineData(1f, null, 1, 10, 4, 5, 0)]
    public async void GetOnSaleStocks_ReturnsCorrectStocks(float? maximumPriceMultiplier, string? stockName, int? pageNumber, int? itemsPerPage,
        float? minimumAverageRating, int stocksOnSale, int expectedReturnedCount)
    {
        //Arrange
        var dataHelper = new DataHelper();
        var stocks = dataHelper.GenerateNewStockTestObjects(stocksOnSale);
        var expectedResult = new PaginatedResult()
        {
            PaginatedItems = expectedReturnedCount > 0 ? stocks.Select(x => x.StockWithRatingData).ToList() : new List<StockWithRatingData>(),
            TotalItems = expectedReturnedCount,
            TotalPages = expectedReturnedCount > 0 ? 1 : 0
        };
        
        await _dataContext.Stocks.AddRangeAsync(stocks.Select(x => x.Stock).ToList());
        await _dataContext.SaveChangesAsync();
        
        var stockManager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);
        
        //Act
        var onSaleStocks = await stockManager.GetOnSaleStocks(maximumPriceMultiplier, stockName, pageNumber, itemsPerPage, minimumAverageRating);
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(onSaleStocks));
    }
    
    #endregion
    #region CreateStock

    [Theory]
    [InlineData("today:0", "today:50", 1000, "{\"decimalMultiplier\":0.5,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:30\"}", 0, 10, 5)]
    [InlineData("today:0", "today:50", 1000, "{\"decimalMultiplier\":0.5,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:50\"}", 0, 10, 5)]
    [InlineData("today:0", "today:50", 1000, "{\"decimalMultiplier\":1,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:10\"}", 0, 10, 5)]
    public async void CreateStock_ValidDetails_ShouldNotThrowException(string createdDate, string expiryDate, float price, string priceMultObjString, int id, int storeId, int categoryId)
    {
        //Arrange
        //format pricemultobjstring
        var daysToAdd = int.Parse(priceMultObjString.Split("\"expiryDate\":")[1].Split(":")[1].Split("\"")[0]);
        var regex = new Regex("today:(-?\\d+)");
        var formattedObjString = regex.Replace(priceMultObjString, DateTime.UtcNow.AddDays(daysToAdd).ToShortDateString());
        //format dates
        DateTime? createdDateTime = createdDate.Contains("today")
            ? DateTime.UtcNow.AddDays(int.Parse(createdDate.Split(":")[1]))
            : null;
        DateTime? expiryDateTime = expiryDate.Contains("today")
            ? DateTime.UtcNow.AddDays(int.Parse(expiryDate.Split(":")[1]))
            : null;

        var stockToAdd = new Stock()
        {
            Id = id,
            CreatedDate = createdDateTime,
            ExpiryDate = expiryDateTime,
            Price = price,
            PriceMultiplierObjString = formattedObjString,
            StoreId = storeId,
            CategoryId = categoryId
        };

        var manager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);

        //Act
        var createdStock = await manager.CreateStock(stockToAdd);
        
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(stockToAdd), JsonConvert.SerializeObject(createdStock));

    }
    
    [Theory]
    [InlineData("null/invalid", "today:50", 1000, "{\"decimalMultiplier\":0.5,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:30\"}", 0, 10, 5)]
     public async void CreateStock_NullCreatedDate_ShouldNotThrowException(string createdDate, string expiryDate, float price, string priceMultObjString, int id, int storeId, int categoryId)
    {
        //Arrange
        //format pricemultobjstring
        var daysToAdd = int.Parse(priceMultObjString.Split("\"expiryDate\":")[1].Split(":")[1].Split("\"")[0]);
        var regex = new Regex("today:(-?\\d+)");
        var formattedObjString = regex.Replace(priceMultObjString, DateTime.UtcNow.AddDays(daysToAdd).ToShortDateString());
        //format dates
        DateTime? createdDateTime = null;
        DateTime? expiryDateTime = expiryDate.Contains("today")
            ? DateTime.UtcNow.AddDays(int.Parse(expiryDate.Split(":")[1]))
            : null;

        var stockToAdd = new Stock()
        {
            Id = id,
            CreatedDate = createdDateTime,
            ExpiryDate = expiryDateTime,
            Price = price,
            PriceMultiplierObjString = formattedObjString,
            StoreId = storeId,
            CategoryId = categoryId
        };

        var expectedStock = new Stock()
        {
            Id = id,
            CreatedDate = DateTime.UtcNow.Date,
            ExpiryDate = expiryDateTime,
            Price = price,
            PriceMultiplierObjString = formattedObjString,
            StoreId = storeId,
            CategoryId = categoryId
        };
        
        var manager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);

        //Act
        var createdStock = await manager.CreateStock(stockToAdd);
        
        //Assert
        Assert.Equal(expectedStock.CreatedDate, createdStock.CreatedDate?.Date);

    }
    
    [Theory]
    [InlineData("today:0", "today:20", 1000, "{\"decimalMultiplier\":0.5,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:30\"}", 0, 10, 5)]
    [InlineData("today:0", "today:50", 0, "{\"decimalMultiplier\":0.5,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:50\"}", 0, 10, 5)]
    [InlineData("today:100", "today:50", 1000, "{\"decimalMultiplier\":1,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:10\"}", 0, 10, 5)]
    [InlineData("today:0", "today:50", 1000, "{\"decimalMultiplier\":1,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:-10000\"}", 0, 10, 5)]
    [InlineData("today:0", "today:50", 1000, "{\"decimalMultiplier\":0.5,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:50\"}", 0, 0, 5)]
    [InlineData("today:0", "today:50", 1000, "{\"decimalMultiplier\":1,\"createdDate\":\"01/01/2010\",\"expiryDate\":\"today:10\"}", 0, 10, 0)]
    public async void CreateStock_InvalidDetails_ShouldThrowException(string createdDate, string expiryDate, float price, string priceMultObjString, int id, int storeId, int categoryId)
    {
        //Arrange
        //format pricemultobjstring
        var daysToAdd = int.Parse(priceMultObjString.Split("\"expiryDate\":")[1].Split(":")[1].Split("\"")[0]);
        var regex = new Regex("today:(-?\\d+)");
        var formattedObjString = regex.Replace(priceMultObjString, DateTime.UtcNow.AddDays(daysToAdd).ToShortDateString());
        //format dates
        DateTime? createdDateTime = createdDate.Contains("today")
            ? DateTime.UtcNow.AddDays(int.Parse(createdDate.Split(":")[1]))
            : null;
        DateTime? expiryDateTime = expiryDate.Contains("today")
            ? DateTime.UtcNow.AddDays(int.Parse(expiryDate.Split(":")[1]))
            : null;

        var stockToAdd = new Stock()
        {
            Id = id,
            CreatedDate = createdDateTime,
            ExpiryDate = expiryDateTime,
            Price = price,
            PriceMultiplierObjString = formattedObjString,
            StoreId = storeId,
            CategoryId = categoryId
        };

        var manager = new StockManager(_mapper, new Logger<StockController>(new LoggerFactory()), _dataContext);

        //Act
        var exception = Record.ExceptionAsync(async () => await manager.CreateStock(stockToAdd));
        
        //Assert
        Assert.NotNull(exception.Result);
    }
    
    #endregion
}