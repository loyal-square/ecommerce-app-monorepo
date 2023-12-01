using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonolithServer;
using MonolithServer.Controllers;
using MonolithServer.Database;
using MonolithServer.Managers.Implementations;
using MonolithServer.Models;
using MonolithServerTest.DataHelpers;
using Profile = MonolithServer.Models.Profile;

namespace MonolithServerTest.Controllers;

public class StockControllerTest
{
    private readonly ApiDbContext _dataContext;
    private readonly IMapper _mapper;

    public StockControllerTest()
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
    public async void StockController_GetAllStocksWithFilters_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.GetAllStocksWithFilters(null, null, null, null, null, null, null));
            
        //Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public async void StockController_GetStocksByStockIds_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.GetStocksByStockIds(new List<int>(), 1, 10));
            
        //Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public async void StockController_GetStocksByStoreId_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.GetStocksByStoreId(0, 1, 10));
            
        //Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public async void StockController_GetOnSaleStocks_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.GetOnSaleStocks(null, null, 1, 10, null));
            
        //Assert
        Assert.Null(exception);
    }

    
    [Fact]
    public async void StockController_CreateStock_ValidCredentials_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        //insert valid profile with store id of 1
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
        
        var exception = await Record.ExceptionAsync(async () => await controller.CreateStock(stock));
            
        //Assert
        Assert.Null(exception);
    }
    [Fact]
    public async void StockController_CreateStock_InvalidCredentials_ShouldThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        //insert valid profile with store id of 1
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = "invalid user name",
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.SaveChangesAsync();
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.CreateStock(stock));
            
        //Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async void StockController_UpdateStock_ValidCredentials_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        await _dataContext.Stocks.AddAsync(stock);
        //insert valid profile with store id of 1
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
        
        var exception = await Record.ExceptionAsync(async () => await controller.UpdateStock(stock, 1));
            
        //Assert
        Assert.Null(exception);
    }
    [Fact]
    public async void StockController_UpdateStock_InvalidCredentials_ShouldThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        //insert valid profile with store id of 1
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = "invalid user name",
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.Stocks.AddAsync(stock);
        //insert valid profile with store id of 1
        await _dataContext.SaveChangesAsync();
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.UpdateStock(stock, 1));
        
        //Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async void StockController_DeleteByStockId_ValidCredentials_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        await _dataContext.Stocks.AddAsync(stock);
        //insert valid profile with store id of 1
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
        
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteByStockId(1));
            
        //Assert
        Assert.Null(exception);
    }
    [Fact]
    public async void StockController_DeleteByStockId_InvalidCredentials_ShouldThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        //insert valid profile with store id of 1
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = "invalid user name",
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.Stocks.AddAsync(stock);
        //insert valid profile with store id of 1
        await _dataContext.SaveChangesAsync();
        //Act
        
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteByStockId(1));
        
        //Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async void StockController_DeleteByStoreId_ValidCredentials_ShouldNotThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        await _dataContext.Stocks.AddAsync(stock);
        //insert valid profile with store id of 1
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
        
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteByStoreId(1));
            
        //Assert
        Assert.Null(exception);
    }
    [Fact]
    public async void StockController_DeleteByStoreId_InvalidCredentials_ShouldThrowException()
    {
        //Arrange
        const string username = "username";
        var fakeClaims = new List<Claim>()
        {
            new Claim("username", username),
        };

        var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");
        var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);
        
        var logger = new Logger<StockController>(new LoggerFactory());
        var manager = new StockManager(_mapper, logger, _dataContext);
        var controller = new StockController(_mapper, logger, manager, _dataContext)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeClaimsPrincipal 
                }
            }
        };
        var dataHelper = new DataHelper();
        var stock = dataHelper.GenerateUniqueStocks()[0];
        //insert valid profile with store id of 1
        await _dataContext.Profiles.AddAsync(new Profile()
        {
            Id = 1,
            Username = "invalid user name",
            StoreIdArrayString = "[\"1\"]",
            Email = "email",
            FirstName = "first",
            LastName = "last",
            ProfileImgUrl = "imgURL",
            UserType = "USER"
        });
        await _dataContext.Stocks.AddAsync(stock);
        //insert valid profile with store id of 1
        await _dataContext.SaveChangesAsync();
        //Act
        foreach (var entity in _dataContext.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
        
        var exception = await Record.ExceptionAsync(async () => await controller.DeleteByStoreId(1));
        
        //Assert
        Assert.NotNull(exception);
    }
}