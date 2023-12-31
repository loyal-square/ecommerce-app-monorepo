﻿using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonolithServer.Controllers;
using MonolithServer.Database;
using MonolithServer.Managers.Interfaces;
using MonolithServer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MonolithServer.Managers.Implementations;

public class StockManager: IStockManager
{
    private readonly IMapper _mapper;
    private readonly ILogger<StockController> _logger;
    private readonly ApiDbContext _context;

    public StockManager(IMapper mapper, ILogger<StockController> logger, ApiDbContext apiDbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _context = apiDbContext;
    }
    public async Task<PaginatedResult> GetAllStocksWithFilters(bool? ascendingNames, bool? ascendingPrices, bool? ascendingCreatedDates,
        bool? onlyAvailable, int? minimumQuantity, int? pageNumber, int? itemsPerPage, string? searchQuery = null, int? categoryId = null)
    {
        var allStocks =  _context.Stocks.ToList();

        if (searchQuery != null)
        {
            allStocks = allStocks.Where(stock => stock.Name?.ToLower().Contains(searchQuery.ToLower()) ?? false).ToList();
        }

        if (categoryId != null)
        {
            allStocks = allStocks.Where(stock => stock.CategoryId.Equals(categoryId)).ToList();
        }

        if (onlyAvailable is true)
        {
            allStocks = allStocks.Where(stock => stock.Available.Equals(true)).ToList();
        }

        if (minimumQuantity is > 0)
        {
            allStocks = allStocks.Where(stock => stock.Quantity >= minimumQuantity).ToList();
        }

        var stocksForServer = _mapper.Map<List<StockForServer>>(allStocks);

        var filteredStocksForServer = new List<StockForServer>();
        if (ascendingNames == true) filteredStocksForServer = stocksForServer.OrderBy(stock => stock.Name).ToList();
        else if (ascendingPrices == true)
            foreach (var stockForServer in stocksForServer)
            {
                var applyPriceMultiplier = stockForServer.PriceMultiplier != null &&
                                           stockForServer.PriceMultiplier.CreatedDate < DateTime.UtcNow &&
                                           stockForServer.PriceMultiplier.ExpiryDate > DateTime.UtcNow;
                if (applyPriceMultiplier)
                    filteredStocksForServer = stocksForServer
                        .OrderBy(stock => stock.Price * stock.PriceMultiplier?.DecimalMultiplier).ToList();
                else
                    filteredStocksForServer = stocksForServer.OrderBy(stock => stock.Price).ToList();
            }
        else if (ascendingCreatedDates == true)
            filteredStocksForServer = stocksForServer.OrderBy(stock => stock.CreatedDate).ToList();
        else
            filteredStocksForServer = stocksForServer;

        itemsPerPage ??= 10;
        pageNumber ??= 1;

        var stocksForClient = _mapper.Map<List<Stock>>(filteredStocksForServer);

        var stocksWithRatingData = await AppendRatingData(stocksForClient, _context);
        return PaginateResults((dynamic)stocksWithRatingData, (int)itemsPerPage, (int)pageNumber);
    }

    public async Task<PaginatedResult> GetStocksByStockIds(List<int> stockIds, int pageNumber, int itemsPerPage)
    {
        var allStocks = await _context.Stocks.Where(stock => stockIds.Contains(stock.Id))
            .ToListAsync();


        var stocksWithRatingData = await AppendRatingData(allStocks, _context);
        return PaginateResults((dynamic)stocksWithRatingData, itemsPerPage, pageNumber);
    }

    public async Task<PaginatedResult> GetStocksByStoreId(int storeId, int pageNumber, int itemsPerPage)
    {
        var allStocks = await _context.Stocks.Where(stock => storeId.Equals(stock.StoreId))
            .ToListAsync();
        var stocksWithRatingData = await AppendRatingData(allStocks, _context);
        return PaginateResults((dynamic)stocksWithRatingData, itemsPerPage, pageNumber);
    }

    public async Task<PaginatedResult> GetOnSaleStocks(float? maximumPriceMultiplier, string? stockName, int? pageNumber, int? itemsPerPage,
        float? minimumAverageRating)
    {
        stockName ??= "";
        maximumPriceMultiplier ??= 1;
        pageNumber ??= 1;
        itemsPerPage ??= 10;

        var onSaleStocks = _mapper.Map<List<StockForServer>>(_context.Stocks);

        onSaleStocks = onSaleStocks
            .Where(stock => stock is { Available: true, Quantity: > 0 } && stock.ExpiryDate > DateTime.UtcNow &&
                            (stock.Name?.Trim().ToLower().Contains(stockName.Trim().ToLower()) ?? false))
            .Where(stock =>
                stock.PriceMultiplier != null &&
                stock.PriceMultiplier.DecimalMultiplier <= maximumPriceMultiplier &&
                stock.PriceMultiplier.CreatedDate <= DateTime.UtcNow &&
                stock.PriceMultiplier.ExpiryDate > DateTime.UtcNow)
            .OrderBy(stock => stock.PriceMultiplier?.DecimalMultiplier).ToList();

        Console.WriteLine(onSaleStocks.ToString());

        var onSaleStocksOutput = _mapper.Map<List<Stock>>(onSaleStocks);
        List<StockWithRatingData> returnObject;
        minimumAverageRating ??= 0;
        if (minimumAverageRating > 0)
            returnObject = await FilterByAverageRating(onSaleStocksOutput, (float)minimumAverageRating, _context);
        else
            returnObject = await AppendRatingData(onSaleStocksOutput, _context);
        return PaginateResults((dynamic)returnObject, (int)itemsPerPage, (int)pageNumber);
    }

    public async Task<Stock> CreateStock(Stock stock)
    {
        stock.CreatedDate ??= DateTime.UtcNow;
        stock.Id = 0;
        
        IsStockDataValid(stock);
        
        var newStock = (await _context.Stocks.AddAsync(stock)).Entity;
        await _context.SaveChangesAsync();
        return newStock;
    }

    public async Task<Stock> UpdateStock(Stock stockValues, int stockId)
    {
        IsStockDataValid(stockValues);
        
        var stockToUpdate = await _context.Stocks.FindAsync(stockId);
        stockToUpdate = stockValues;
        await _context.SaveChangesAsync();
        return stockToUpdate;
    }

    public async Task DeleteByStockObject(Stock stock)
    {
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByStoreId(int storeId)
    {
        var stocksToRemove = await _context.Stocks
            .Where(stock => stock.StoreId.Equals(storeId)).ToListAsync();
        _context.Stocks.RemoveRange(stocksToRemove);
        await _context.SaveChangesAsync();
    }
    
    #region private

    private void IsStockDataValid(Stock stock)
    {
        //check for invalid data before creation
        if (stock.ExpiryDate < stock.CreatedDate){
            throw new DataException("Invalid expiry date.");
        }

        if (stock.Price is 0)
        {
            throw new DataException("The price must be greater than 0.");
        }

        if (stock.PriceMultiplierObjString is not null)
        {
            var format = "d/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var decimalMultiplierObj = JsonConvert.DeserializeObject<PriceMultiplier>(stock.PriceMultiplierObjString ?? "{}", dateTimeConverter);
            if (decimalMultiplierObj?.DecimalMultiplier is 0 or > 1)
            {
                throw new DataException("The price multiplier must be greater than 0 and less than 1.");
            }

            if (decimalMultiplierObj?.CreatedDate > decimalMultiplierObj?.ExpiryDate)
            {
                throw new DataException("The price multiplier created date must be less than its expriry date.");
            }
        
            if (stock.ExpiryDate < decimalMultiplierObj?.ExpiryDate)
            {
                throw new DataException("The price multiplier expiry date must be less than the stock's expriry date.");
            }
        }
        

        if (stock.CategoryId is 0)
        {
            throw new DataException("Invalid category Id 0.");
        }
        
        if (stock.StoreId is 0)
        {
            throw new DataException("Invalid store Id 0.");
        }
    }
    private PaginatedResult PaginateResults(dynamic unpaginatedItems, int itemsPerPage, int pageNumber)
    {
        dynamic paginatedItems = new List<dynamic>();
        var currentPage = 1;
        var currentItemCounter = 1;

        for (var i = 0; i < unpaginatedItems.Count; i++)
            if (currentItemCounter <= itemsPerPage)
            {
                if (currentPage == pageNumber) paginatedItems.Add(unpaginatedItems[i]);

                currentItemCounter++;
            }
            else
            {
                i--;
                currentPage++;
                currentItemCounter = 1;
            }

        return new PaginatedResult
        {
            PaginatedItems = paginatedItems,
            TotalItems = unpaginatedItems.Count ?? 0,
            TotalPages = (int)Math.Ceiling((unpaginatedItems.Count ?? 0) / (float)itemsPerPage)
        };
    }

    private static StockWithRatingData MapRatingData(Stock stock, float averageRating, int numberOfRatings)
    {
        return new StockWithRatingData
        {
            Id = stock.Id,
            Name = stock.Name,
            Price = stock.Price,
            Currency = stock.Currency,
            Description = stock.Description,
            Details = stock.Details,
            Available = stock.Available,
            Quantity = stock.Quantity,
            CategoryId = stock.CategoryId,
            StoreId = stock.StoreId,
            PriceMultiplierObjString = stock.PriceMultiplierObjString,
            CreatedDate = stock.CreatedDate,
            ExpiryDate = stock.ExpiryDate,
            AverageRating = averageRating,
            NumberOfRatings = numberOfRatings
        };
    }

    private static async Task<List<StockWithRatingData>> AppendRatingData(List<Stock> allStocks, ApiDbContext context)
    {
        var matchingStocks = new List<StockWithRatingData>();
        foreach (var stock in allStocks)
        {
            var stockRatings = await context.StockRatings.Where(x => x.StockId.Equals(stock.Id))
                .ToListAsync();
            if (stockRatings.Count <= 0)
            {
                var stockWithRatingData = MapRatingData(stock, 0, 0);

                matchingStocks.Add(stockWithRatingData);
            }
            else
            {
                var averageStockRating = stockRatings.Average(x => x.RatingValue);
                var stockWithRatingData = MapRatingData(stock, averageStockRating, stockRatings.Count);

                matchingStocks.Add(stockWithRatingData);
            }
        }

        return matchingStocks;
    }

    private static async Task<List<StockWithRatingData>> FilterByAverageRating(List<Stock> allStocks,
        float minimumAverageRating, ApiDbContext context)
    {
        var matchingStocks = new List<StockWithRatingData>();
        foreach (var stock in allStocks)
        {
            var stockRatings = await context.StockRatings.Where(x => x.StockId.Equals(stock.Id))
                .ToListAsync();
            if (stockRatings.Count <= 0) continue;
            var averageStockRating = stockRatings.Average(x => x.RatingValue);
            if (averageStockRating >= minimumAverageRating)
            {
                var stockWithRatingData = MapRatingData(stock, averageStockRating, stockRatings.Count);

                matchingStocks.Add(stockWithRatingData);
            }
        }

        return matchingStocks;
    }

    #endregion
}