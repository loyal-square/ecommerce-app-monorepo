using System.Data;
using Microsoft.EntityFrameworkCore;
using MonolithServer.Database;
using MonolithServer.Managers.Interfaces;
using MonolithServer.Models;

namespace MonolithServer.Managers.Implementations;

public class RatingsManager: IRatingsManager
{
    private readonly ApiDbContext _context;
    private IRatingsManager _ratingsManagerImplementation;

    public RatingsManager(ApiDbContext context)
    {
        _context = context;
    }
    public async Task<List<StockRating>> GetStockRatingsByStockIds(List<int> stockIds)
    {
        var allStockRatings = await _context.StockRatings.Where(stockRating => stockIds.Contains(stockRating.StockId)).ToListAsync();
        
        return allStockRatings;
    }

    public async Task<List<StockRating>> GetStockRatingsByStoreId(int storeId)
    {
        var allStocksRatings = await _context.StockRatings.Where(stockRating => storeId.Equals(stockRating.StoreId)).ToListAsync();
        return allStocksRatings;
    }

    public async Task<AverageStockRatings> GetAverageStockRatingByStock(int stockId)
    {
        var allStocksRatings = await _context.StockRatings.Where(stockRating => stockId.Equals(stockRating.StockId)).ToListAsync();
        var sumOfStockRatings = allStocksRatings.Sum(t => t.RatingValue);

        if (allStocksRatings.Count.Equals(0))
        {
            return new AverageStockRatings()
            {
                AverageRating = 0, 
                StockId = stockId, 
                RatingsCount = 0
            };
        }
            
        return new AverageStockRatings()
        {
            AverageRating = sumOfStockRatings / allStocksRatings.Count, 
            StockId = stockId, 
            RatingsCount = allStocksRatings.Count
        };
    }

    public async Task<List<AverageStockRatings>> GetAverageStockRatingsByStockIds(List<int> stockIds)
    {
        var averageStockRatings = new List<AverageStockRatings>();
            
        for (var i = 0; i < stockIds.Count; i++)
        {
            var stockId = stockIds[i];
            var allStocksRatings = await _context.StockRatings.Where(stockRating => stockId.Equals(stockRating.StockId)).ToListAsync();
            var sumOfStockRatings = allStocksRatings.Sum(t => t.RatingValue);

            if (allStocksRatings.Count.Equals(0))
            {
                averageStockRatings.Add(new AverageStockRatings()
                {
                    AverageRating = 0, 
                    StockId = stockId, 
                    RatingsCount = 0
                });
            }
            else
            {
                averageStockRatings.Add(new AverageStockRatings()
                {
                    AverageRating = sumOfStockRatings/allStocksRatings.Count,
                    StockId = stockId,
                    RatingsCount = allStocksRatings.Count
                });
            }
        }

        return averageStockRatings;
    }

    public async Task<StockRating?> CreateStockRating(StockRating stockRating)
    {
        //Find if stockRating userId matches any existing for the related stock. If it does, prevent creation of a new rating.
        var allStockRatings = await _context.StockRatings.ToListAsync();
        var matchingStockRating = allStockRatings.Find(s => s.StockId.Equals(stockRating.StockId) && s.UserId.Equals(stockRating.UserId));
        if (matchingStockRating != null)
        {
            throw new Exception("You cannot create a new rating for a stock you have already rated before. You can however edit your existing rating.");
        }

        var newStockRating = _context.StockRatings.Add(stockRating).Entity;
        await _context.SaveChangesAsync();
        return newStockRating;
    }

    public async Task DeleteStockRating(StockRating? stockRatingToDelete)
    {
        if (stockRatingToDelete == null) return;
        _context.StockRatings.Remove(stockRatingToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStockRatingByStoreId(int storeId)
    {
        var stockRatingsToDelete = await _context.StockRatings.Where(rating => rating.StoreId.Equals(storeId)).ToListAsync();
        if (stockRatingsToDelete.Count == 0) return;
        _context.StockRatings.RemoveRange(stockRatingsToDelete);
        await _context.SaveChangesAsync();
    }
}