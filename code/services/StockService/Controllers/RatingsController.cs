using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockService.Database;
using StockService.Models;

namespace StockService.Controllers
{
    [ApiController]
    [Route("api/Rating")]
    public class RatingsController : ControllerBase
    {
        [HttpGet]
        [Route("StockIds")]
        public async Task<List<StockRating>> GetStockRatingsByStockIds([FromQuery] string[] stockIds)
        {
            var allStockRatings = await DbInitializer.context.StockRatings.Where(stockRating => stockIds.Contains(stockRating.StockId.ToString())).ToListAsync();
            return allStockRatings;
        }

        [HttpGet]
        [Route("StoreId")]
        public async Task<List<StockRating>> GetStockRatingsByStoreId([FromQuery] int storeId)
        {
            var allStocksRatings = await DbInitializer.context.StockRatings.Where(stockRating => storeId.Equals(stockRating.StoreId)).ToListAsync();
            return allStocksRatings;
        }

        [HttpPut]
        [Authorize]
        [Route("create")]
        public async Task<StockRating?> CreateStockRating([FromBody] StockRating stockRating)
        {
            //Find if stockRating userId matches any existing for the related stock. If it does, prevent creation of a new rating.
            var allStockRatings = await DbInitializer.context.StockRatings.ToListAsync();
            var matchingStockRating = allStockRatings.Find(s => s.StockId.Equals(stockRating.StockId) && s.UserId.Equals(stockRating.UserId));
            if (matchingStockRating != null)
            {
                throw new Exception("You cannot create a new rating for a stock you have already rated before. You can however edit your existing rating.");
            }

            var newStockRating = DbInitializer.context.StockRatings.Add(stockRating).Entity;
            await DbInitializer.context.SaveChangesAsync();
            return newStockRating;
        }

        [HttpDelete]
        [Authorize]
        [Route("stockRatingId/{stockRatingId:int}")]
        public async void DeleteStockRatingById(int stockRatingId)
        {
            var stockRatingToDelete = await DbInitializer.context.StockRatings.FindAsync(stockRatingId);
            if (stockRatingToDelete == null) return;
            DbInitializer.context.StockRatings.Remove(stockRatingToDelete);
            await DbInitializer.context.SaveChangesAsync();
        }
        
        [HttpDelete]
        [Authorize]
        [Route("StoreId/{StoreId:int}")]
        public async void DeleteStockRatingByStoreId(int storeId)
        {
            var stockRatingsToDelete = await DbInitializer.context.StockRatings.Where(rating => rating.StoreId.Equals(storeId)).ToListAsync();
            if (stockRatingsToDelete.Count == 0) return;
            DbInitializer.context.StockRatings.RemoveRange(stockRatingsToDelete);
            await DbInitializer.context.SaveChangesAsync();
        }
    }
}

