using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonolithServer.Database;
using MonolithServer.Helpers;
using MonolithServer.Managers.Interfaces;
using MonolithServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonolithServer.Controllers;

[ApiController]
[Route("api/Stock")]
public class StockController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<StockController> _logger;
    private readonly IStockManager _stockManager;
    private readonly ApiDbContext _context;

    public StockController(IMapper mapper, ILogger<StockController> logger, IStockManager stockManager, ApiDbContext context)
    {
        _mapper = mapper;
        _logger = logger;
        _stockManager = stockManager;
        _context = context;
    }
    
    [HttpGet]
    [Route("all/filters")]
    public async Task<PaginatedResult> GetAllStocksWithFilters([FromQuery] bool? ascendingNames,
        [FromQuery] bool? ascendingPrices, [FromQuery] bool? ascendingCreatedDates, [FromQuery] bool? onlyAvailable,
        [FromQuery] int? minimumQuantity, [FromQuery] int? pageNumber,
        [FromQuery] int? itemsPerPage)
    {
        return await _stockManager.GetAllStocksWithFilters(ascendingNames, ascendingPrices, ascendingCreatedDates, onlyAvailable,
            minimumQuantity, pageNumber, itemsPerPage);
    }

    [HttpGet]
    [Route("byStockIds")]
    public async Task<PaginatedResult> GetStocksByStockIds([FromQuery] string[] stockIds,
        [FromQuery] int pageNumber, [FromQuery] int itemsPerPage)
    {
        return await _stockManager.GetStocksByStockIds(stockIds, pageNumber, itemsPerPage);
    }

    [HttpGet]
    [Route("byStoreId")]
    public async Task<PaginatedResult> GetStocksByStoreId([FromQuery] int storeId, [FromQuery] int pageNumber,
        [FromQuery] int itemsPerPage)
    {
        return await _stockManager.GetStocksByStoreId(storeId, pageNumber, itemsPerPage);
    }

    [HttpGet]
    [Route("onsale")]
    public async Task<PaginatedResult> GetOnSaleStocks([FromQuery] float? minimumPriceMultiplier,
        [FromQuery] string? stockName, [FromQuery] int? pageNumber, [FromQuery] int? itemsPerPage,
        [FromQuery] float? minimumAverageRating)
    {
        return await _stockManager.GetOnSaleStocks(minimumPriceMultiplier, stockName, pageNumber, itemsPerPage,
            minimumAverageRating);
    }

    [HttpPut]
    [Authorize]
    [Route("create")]
    public async Task<Stock> CreateStock([FromBody] Stock stock)
    {
        //compare the username in the jwt to the usernames in the store the stock is from
        if (await AuthHelpers.AccessingRestrictedStockData(User, stock, _context))
        {
            //accessing restricted resources means inability to create stock
            throw new UnauthorizedAccessException("Attempted to access restricted resources. This is forbidden.");
        }
        

        //execute stock creation
        return await _stockManager.CreateStock(stock);
    }

    [HttpPut]
    [Authorize]
    [Route("update/{stockId:int}")]
    public async Task<Stock> UpdateStock([FromBody] Stock stockValues, int stockId)
    {
        if (await AuthHelpers.AccessingRestrictedStockData(User, stockValues, _context))
        {
            throw new UnauthorizedAccessException("Attempted to access restricted resources. This is forbidden.");
        }

        return await _stockManager.UpdateStock(stockValues, stockId);
    }

    [HttpDelete]
    [Authorize]
    [Route("stockId/{stockId:int}")]
    public async Task DeleteByStockId(int stockId)
    {
        var user = User;
        var stock = await _context.Stocks.FindAsync(stockId);
        
        if (stock != null)
        {
            if (await AuthHelpers.AccessingRestrictedStockData(user, stock, _context))
            {
                throw new UnauthorizedAccessException("Attempted to access restricted resources. This is forbidden.");
            }

            await _stockManager.DeleteByStockId(stock);
        }
        else {
            throw new Exception($"Stock with ID {stockId} not found");
        }
    }

    [HttpDelete]
    [Authorize]
    [Route("storeId/{storeId:int}")]
    public async Task DeleteByStoreId(int storeId)
    {
        if (await AuthHelpers.AccessingRestrictedDataByStoreId(User, storeId, _context))
        {
            throw new UnauthorizedAccessException("Attempted to access restricted resources. This is forbidden.");
        }

        await _stockManager.DeleteByStoreId(storeId);
    }
}