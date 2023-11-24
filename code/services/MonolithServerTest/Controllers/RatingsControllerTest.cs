using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MonolithServer;
using MonolithServer.Controllers;
using MonolithServer.Database;
using MonolithServer.Managers.Implementations;
using MonolithServer.Models;
using Newtonsoft.Json;
using Profile = MonolithServer.Models.Profile;

namespace MonolithServerTest.Controllers;

public class RatingsControllerTest
{
    private readonly ApiDbContext _dataContext;
    private readonly IMapper _mapper;

    public RatingsControllerTest()
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
    
    
    [Fact]
    public async void GetStockRatingsByStockIds_NoCredentials_ShouldSucceed()
    {
        //arrange
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 2,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
        };

        var stockRatingsReturned = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 4,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            }
        };
        
        stockRatings.AddRange(stockRatingsReturned);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.GetStockRatingsByStockIds(stockRatingsReturned.Select(x => x.StockId).ToList()));
        
        //Assert
        Assert.Null(exception?.Message);

    }
    
    [Fact]
    public async void GetStockRatingsByStoreId_NoCredentials_ShouldSucceed()
    {
        //arrange
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 2,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
        };

        var stockRatingsReturned = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 2,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 4,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 2,
                UserId = 1,
            }
        };
        
        stockRatings.AddRange(stockRatingsReturned);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.GetStockRatingsByStoreId(stockRatingsReturned[0].StoreId));
        
        //Assert
        Assert.Null(exception?.Message);

    }
    
    [Fact]
    public async void GetAverageStockRatingByStock_NoCredentials_ShouldSucceed()
    {
        //arrange
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 2,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
        };

        var stockRatingsReturned = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 2,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 4,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 2,
                UserId = 1,
            }
        };
        
        stockRatings.AddRange(stockRatingsReturned);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.GetAverageStockRatingByStock(stockRatingsReturned[0].StockId));
        
        //Assert
        Assert.Null(exception?.Message);
        
    }
    
    [Fact]
    public async void GetAverageRatingByStockIds_NoCredentials_ShouldSucceed()
    {
        //arrange
        var stockRatings = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 1,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 2,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
        };

        var stockRatingsReturned = new List<StockRating>()
        {
            new StockRating()
            {
                Id = 3,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            },
            new StockRating()
            {
                Id = 4,
                StockId = 2,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            }
        };
        
        stockRatings.AddRange(stockRatingsReturned);

        await _dataContext.StockRatings.AddRangeAsync(stockRatings);
        await _dataContext.SaveChangesAsync();

        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.GetAverageStockRatingsByStockIds(stockRatingsReturned.Select(x => x.StockId).ToList()));
        
        //Assert
        Assert.Null(exception?.Message);

    }
    
    [Fact]
    public async void CreateStockRating_NoCredentials_ShouldThrowException()
    {
        //arrange
        var stockRating = new StockRating()
            {
                Id = 2,
                StockId = 1,
                RatingValue = 5,
                RatedDate = DateTime.UtcNow,
                StoreId = 1,
                UserId = 1,
            };

        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.CreateStockRating(stockRating));
        
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Attempted to access restricted data. This is not allowed");

    }
    
    [Fact]
    public async void CreateStockRating_WithCredentials_ShouldNotThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 2,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager){
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = username,
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.SaveChangesAsync();
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.CreateStockRating(stockRating));
        
        //Assert
        Assert.Null(exception?.Message);
    }
    
    [Fact]
    public async void DeleteStockRatingById_WithNoCredentials_ShouldThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 2,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
      
        
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        await _dataContext.StockRatings.AddAsync(stockRating);
       
        await _dataContext.SaveChangesAsync();
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteStockRatingById(stockRating.Id));
        
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Attempted to access restricted data. This is not allowed");
    }
    
    [Fact]
    public async void DeleteStockRatingById_WithInvalidCredentials_ShouldThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 2,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager){
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        await _dataContext.StockRatings.AddAsync(stockRating);
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = username + "invalid",
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.SaveChangesAsync();
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteStockRatingById(stockRating.Id));
        
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Attempted to access restricted data. This is not allowed");
    }
    
    [Fact]
    public async void DeleteStockRatingById_WithCredentials_ShouldNotThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 2,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager){
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        await _dataContext.StockRatings.AddAsync(stockRating);
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = username,
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.SaveChangesAsync();
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteStockRatingById(stockRating.Id));
        
        //Assert
        Assert.Null(exception?.Message);
    }
    
    [Fact]
    public async void DeleteStockRatingByStoreId_WithNoCredentials_ShouldThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 2,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
      
        
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager);
        await _dataContext.StockRatings.AddAsync(stockRating);
       
        await _dataContext.SaveChangesAsync();
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteStockRatingByStoreId(stockRating.StoreId));
        
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Attempted to access restricted data. This is not allowed");
    }
    
    [Fact]
    public async void DeleteStockRatingByStoreId_WithInvalidCredentials_ShouldThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 2,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager){
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        await _dataContext.StockRatings.AddAsync(stockRating);
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = username + "invalid",
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.SaveChangesAsync();
        //Act
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteStockRatingByStoreId(stockRating.StoreId));
        
        //Assert
        Assert.NotNull(exception?.Message);
        Assert.Contains(exception?.Message, "Attempted to access restricted data. This is not allowed");
    }
    
    [Fact]
    public async void DeleteStockRatingByStoreId_WithCredentials_ShouldNotThrowException()
    {
        //arrange
        var stockRating = new StockRating()
        {
            Id = 1,
            StockId = 1,
            RatingValue = 5,
            RatedDate = DateTime.UtcNow,
            StoreId = 1,
            UserId = 1,
        };
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        var manager = new RatingsManager(_dataContext);
        var controller = new RatingsController(_dataContext, manager){
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        await _dataContext.StockRatings.AddAsync(stockRating);
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = username,
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.SaveChangesAsync();
        //Act
        foreach (var entity in _dataContext.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteStockRatingByStoreId(stockRating.StoreId));
        
        //Assert
        Assert.Null(exception?.Message);
    }
}