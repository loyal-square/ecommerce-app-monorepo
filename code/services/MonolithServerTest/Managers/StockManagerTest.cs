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
                Name = "Z"
            },
            new Stock()
            {
                Id = 2,
                Name = "A"
            }
        };
        
        var orderedStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 2,
                Name = "A"
            },
            new StockWithRatingData()
            {
                Id = 1,
                Name = "Z"
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
                Price = 1000
            },
            new Stock()
            {
                Id = 2,
                Name = "B",
                Price = 500
            }
        };
        
        var orderedStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 2,
                Name = "B",
                Price = 500
            },
            new StockWithRatingData()
            {
                Id = 1,
                Name = "A",
                Price = 1000
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
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(3)
            },
            new Stock()
            {
                Id = 2,
                Name = "B",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(2)
            },
            new Stock()
            {
                Id = 3,
                Name = "C",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(1)
            }
        };
        
        var orderedStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 3,
                Name = "C",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(1)
            },
            new StockWithRatingData()
            {
                Id = 2,
                Name = "B",
                Price = 1000,
                PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(2)
            },
             new StockWithRatingData()
             {
                 Id = 1,
                 Name = "A",
                 Price = 1000,
                 PriceMultiplierObjString = DataHelper.GenerateNewPriceMultiplier(3)
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
                Name = "Z"
            },
            new Stock()
            {
                Id = 2,
                Available = false,
                Name = "A"
            }
        };

        var correctStock = new List<StockWithRatingData>()
        {
            new StockWithRatingData()
            {
                Id = 1,
                Name = "Z",
                Available = true,
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
}