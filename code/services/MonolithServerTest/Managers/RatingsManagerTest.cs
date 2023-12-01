using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonolithServer;
using MonolithServer.Database;
using MonolithServer.Managers.Implementations;
using MonolithServer.Models;
using Newtonsoft.Json;

namespace MonolithServerTest.Managers;

public class RatingsManagerTest
{
    private readonly ApiDbContext _dataContext;

    public RatingsManagerTest()
    {
        DbContextOptions<ApiDbContext> dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        _dataContext = new ApiDbContext(dbContextOptions);
    }

    [Fact]
    public async void GetStockRatingsByStockIds_ValidIds_ReturnsRatings()
    {
        //Arrange
        var ids = new List<int>()
        {
            1
        };
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1,
                UserId = 3
            }
        };
        var expectedRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 2
            },
        };
        stockRatings.AddRange(expectedRatings);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetStockRatingsByStockIds(ids);
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(expectedRatings));
    }
    
    [Fact]
    public async void GetStockRatingsByStockIds_InvalidIds_ReturnsEmptyList()
    {
        //Arrange
        var ids = new List<int>()
        {
            3
        };
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1,
                UserId = 3
            },
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 2
            },
        };

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetStockRatingsByStockIds(ids);
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(new List<StockRating>()));
        Assert.Empty(response);
    }

    [Fact]
    public async void GetStockRatingsByStoreId_ValidStoreId_ReturnsRatings()
    {
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 2,
                UserId = 3
            }
        };
        var expectedRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 2
            },
        };
        stockRatings.AddRange(expectedRatings);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetStockRatingsByStoreId(1);
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(expectedRatings));
    }
    [Fact]
    public async void GetStockRatingsByStoreId_InvalidStoreId_ReturnsEmptyList()
    {
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 2,
                UserId = 3
            },
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 2
            },
        };

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetStockRatingsByStoreId(5);
        //Assert
        Assert.Equal(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(new List<StockRating>()));
        Assert.Empty(response);
    }

    [Fact]
    public async void GetAverageStockRatingByStock_ValidStockId_ReturnsCorrectObject()
    {
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                RatingValue = 3,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 2,
                UserId = 3
            },
            new StockRating()
            {
                Id = 1,
                RatingValue = 4,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 2
            },
        };

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetAverageStockRatingByStock(1);
        //Assert
        Assert.Equal(4, response.AverageRating);
        Assert.Equal(1, response.StockId);
        Assert.Equal(3, response.RatingsCount);
    }
    
    [Fact]
    public async void GetAverageStockRatingByStock_NotValidStockId_ReturnsCorrectAverage()
    {
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                RatingValue = 3,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 2,
                UserId = 3
            },
            new StockRating()
            {
                Id = 1,
                RatingValue = 4,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 2
            },
        };

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetAverageStockRatingByStock(3);
        //Assert
        Assert.Equal(0, response.AverageRating);
        Assert.Equal(3, response.StockId);
        Assert.Equal(0, response.RatingsCount);
    }

    [Fact]
    public async void GetAverageStockRatingsByStockIds_ValidIds_ReturnsCorrectList()
    {
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 5,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 3,
                StoreId = 2,
                UserId = 3
            }
        };
        var expectedRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 3,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 3,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1,
                UserId = 2
            },
            new StockRating()
            {
                Id = 4,
                RatingValue = 3,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1,
                UserId = 2
            },
        };
        stockRatings.AddRange(expectedRatings);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetAverageStockRatingsByStockIds(new List<int>(){1,2});
        //Assert
        var firstRating = response[0];
        var secondRating = response[1];
        Assert.Equal(4, firstRating.AverageRating);
        Assert.Equal(1, firstRating.StockId);
        Assert.Equal(2, firstRating.RatingsCount);
        
        Assert.Equal(4, secondRating.AverageRating);
        Assert.Equal(2, secondRating.StockId);
        Assert.Equal(2, secondRating.RatingsCount);
    }
    
    [Fact]
    public async void GetAverageStockRatingsByStockIds_NotValidIds_ReturnsCorrectList()
    {
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 5,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 3,
                StoreId = 2,
                UserId = 3
            },
            new StockRating()
            {
                Id = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 2,
                RatingValue = 3,
                RatedDate = DateTime.UtcNow,
                StockId = 1,
                StoreId = 1,
                UserId = 1
            },
            new StockRating()
            {
                Id = 3,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1,
                UserId = 2
            },
            new StockRating()
            {
                Id = 4,
                RatingValue = 3,
                RatedDate = DateTime.UtcNow,
                StockId = 2,
                StoreId = 1,
                UserId = 2
            },
        };

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        
        //Act
        var response = await manager.GetAverageStockRatingsByStockIds(new List<int>(){5,6});
        //Assert
        var firstRating = response[0];
        var secondRating = response[1];
        Assert.Equal(0, firstRating.AverageRating);
        Assert.Equal(5, firstRating.StockId);
        Assert.Equal(0, firstRating.RatingsCount);
        
        Assert.Equal(0, secondRating.AverageRating);
        Assert.Equal(6, secondRating.StockId);
        Assert.Equal(0, secondRating.RatingsCount);
        
    }

    [Fact]
    public async void CreateStockRating_ValidRating_Succeeds()
    {
        //Arrange
        var rating = new StockRating()
        {
            RatingValue = 4,
            StockId = 1,
            StoreId = 1
        };
        var manager = new RatingsManager(_dataContext);
        //Act
        var newRating = await manager.CreateStockRating(rating);
        //Assert
        Assert.Equal(4 , newRating.RatingValue);
        Assert.Equal(DateTime.UtcNow.Date , newRating.RatedDate.Date);
        Assert.Equal(1 , newRating.StockId);
        Assert.Equal(1 , newRating.StoreId);
    }
    [Fact]
    public async void CreateStockRating_NotValidRating_ThrowsException()
    {
        //Arrange
        var rating = new StockRating()
        {
            RatingValue = 10,
            StockId = 1,
            StoreId = 1
        };
        var manager = new RatingsManager(_dataContext);
        //Act
        var exception = await Record.ExceptionAsync(async () => await manager.CreateStockRating(rating)); 
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Rating value can't exceed 5.");

    }

    [Fact]
    public async void CreateStockRating_DuplicateRating_ThrowsException()
    {
        //Arrange
        var dup = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };
        
        var rating = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };

        await _dataContext.StockRatings.AddAsync(dup);
        await _dataContext.SaveChangesAsync();
        
        var manager = new RatingsManager(_dataContext);
        //Act
        var exception = await Record.ExceptionAsync(async () => await manager.CreateStockRating(dup)); 
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "You cannot create a new rating for a stock you have already rated before. You can however edit your existing rating.");
    }

    [Fact]
    public async void DeleteStockRating_NonNullRating_Succeeds()
    {
        //Arrange
        
        var rating = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };

        await _dataContext.StockRatings.AddAsync(rating);
        await _dataContext.SaveChangesAsync();
        
        var manager = new RatingsManager(_dataContext);
        //Act
        await manager.DeleteStockRating(rating);
        //Assert
        Assert.Empty(_dataContext.StockRatings);
        
    }
    [Fact]
    public async void DeleteStockRating_NullRating_DoesNotThrowException()
    {
        //Arrange
        var manager = new RatingsManager(_dataContext);
        //Act
        var exception = await Record.ExceptionAsync(async () => await manager.DeleteStockRating(null)); 
        //Assert
        Assert.Null(exception?.Message);
    }
    [Fact]
    public async void DeleteStockRatingByStoreId_NonNullRating_Succeeds()
    {
        //Arrange
        
        var rating = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };

        await _dataContext.StockRatings.AddAsync(rating);
        await _dataContext.SaveChangesAsync();
        
        var manager = new RatingsManager(_dataContext);
        //Act
        foreach (var entity in _dataContext.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
        await manager.DeleteStockRatingByStoreId(1);
        //Assert
        Assert.Empty(_dataContext.StockRatings);
        
    }
    [Fact]
    public async void DeleteStockRatingByStoreId_InvalidStoreId_DoesNotThrowException()
    {
        //Arrange
        var manager = new RatingsManager(_dataContext);
        //Act
        var exception = await Record.ExceptionAsync(async () => await manager.DeleteStockRatingByStoreId(1)); 
        //Assert
        Assert.Null(exception?.Message);
    }

    [Fact]
    public async void UpdateStockRatings_ValidRating_Succeeds()
    {
        //Arrange
        var rating = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };

        await _dataContext.StockRatings.AddAsync(rating);
        await _dataContext.SaveChangesAsync();
        
        var manager = new RatingsManager(_dataContext);

        rating.Id = 1;
        rating.RatingValue = 3;
        //Act
        await manager.UpdateStockRating(rating);
        //Assert
        var updated = await _dataContext.StockRatings.FindAsync(1);
        Assert.Equal(3, updated?.RatingValue);
    }
    [Fact]
    public async void UpdateStockRatings_NotValidRating_ThrowsException()
    {
        //Arrange
        var rating = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };

        await _dataContext.StockRatings.AddAsync(rating);
        await _dataContext.SaveChangesAsync();
        
        var manager = new RatingsManager(_dataContext);

        rating.Id = 1;
        rating.RatingValue = 10;
        //Act
        var exception = await Record.ExceptionAsync(async () =>  await manager.UpdateStockRating(rating));
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Rating value can't exceed 5.");
    }
    [Fact]
    public async void UpdateStockRatings_NotExistentRating_DoesNotThrowException()
    {
        //Arrange
        var rating = new StockRating()
        {
            RatingValue = 5,
            StockId = 1,
            StoreId = 1,
            UserId = 1
        };
        
        var manager = new RatingsManager(_dataContext);
        
        //Act
        var exception = await Record.ExceptionAsync(async () =>  await manager.UpdateStockRating(rating));
        //Assert
        Assert.Null(exception?.Message);
    }
}