using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonolithServer.Database;
using MonolithServer.Helpers;
using MonolithServer.Models;

namespace MonolithServer.Controllers
{
    [ApiController]
    [Route("api/Rating")]
    public class RatingsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public RatingsController(ApiDbContext apiDbContext)
        {
            _context = apiDbContext;
        }
    [HttpGet]
        [Route("StockIds")]
        public async Task<List<StockRating>> GetStockRatingsByStockIds([FromQuery] string[] stockIds)
        {
            var allStockRatings = await _context.StockRatings.Where(stockRating => stockIds.Contains(stockRating.StockId.ToString())).ToListAsync();
            return allStockRatings;
        }

        [HttpGet]
        [Route("StoreId")]
        public async Task<List<StockRating>> GetStockRatingsByStoreId([FromQuery] int storeId)
        {
            var allStocksRatings = await _context.StockRatings.Where(stockRating => storeId.Equals(stockRating.StoreId)).ToListAsync();
            return allStocksRatings;
        }
        
        [HttpGet]
        [Route("averageByStock")]
        public async Task<AverageStockRatings> GetAverageStockRatingByStock([FromQuery] int stockId)
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
        
        [HttpGet]
        [Route("averagesByStocks")]
        public async Task<List<AverageStockRatings>> GetAverageStockRatingsByStockIds([FromQuery] int[] stockIds)
        {
            var averageStockRatings = new List<AverageStockRatings>();
            
            for (var i = 0; i < stockIds.Length; i++)
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

        [HttpPut]
        [Authorize]
        [Route("create")]
        public async Task<StockRating?> CreateStockRating([FromBody] StockRating stockRating)
        {
            if (await AuthHelpers.AccessingRestrictedRatingsData(User, stockRating, _context))
            {
                throw new UnauthorizedAccessException("Attempted to access restricted data. This is not allowed");
            }
            
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

        [HttpDelete]
        [Authorize]
        [Route("stockRatingId/{stockRatingId:int}")]
        public async Task DeleteStockRatingById(int stockRatingId)
        {
            var user = User;
            var stockRatingToDelete = await _context.StockRatings.FindAsync(stockRatingId);
            if (await AuthHelpers.AccessingRestrictedRatingsData(user, stockRatingToDelete, _context))
            {
                throw new UnauthorizedAccessException("Attempted to access restricted data. This is not allowed");
            }
            
            if (stockRatingToDelete == null) return;
            _context.StockRatings.Remove(stockRatingToDelete);
            await _context.SaveChangesAsync();
        }
        
        [HttpDelete]
        [Authorize]
        [Route("StoreId/{storeId:int}")]
        public async Task DeleteStockRatingByStoreId(int storeId)
        {
            if (await AuthHelpers.AccessingRestrictedDataByStoreId(User, storeId, _context))
            {
                throw new UnauthorizedAccessException("Attempted to access restricted data. This is not allowed");
            }
            
            var stockRatingsToDelete = await _context.StockRatings.Where(rating => rating.StoreId.Equals(storeId)).ToListAsync();
            if (stockRatingsToDelete.Count == 0) return;
            _context.StockRatings.RemoveRange(stockRatingsToDelete);
            await _context.SaveChangesAsync();
        }
    }
}

