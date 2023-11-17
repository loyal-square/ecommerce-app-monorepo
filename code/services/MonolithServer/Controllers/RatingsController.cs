using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolithServer.Database;
using MonolithServer.Helpers;
using MonolithServer.Managers.Interfaces;
using MonolithServer.Models;

namespace MonolithServer.Controllers
{
    [ApiController]
    [Route("api/Rating")]
    public class RatingsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IRatingsManager _ratingsManager;
        public RatingsController(ApiDbContext apiDbContext, IRatingsManager ratingsManager)
        {
            _context = apiDbContext;
            _ratingsManager = ratingsManager;
        }
        [HttpPost]
        [Route("StockIds")]
        public async Task<List<StockRating>> GetStockRatingsByStockIds([FromBody] List<int> stockIds)
        {
            return await _ratingsManager.GetStockRatingsByStockIds(stockIds);
        }

        [HttpGet]
        [Route("StoreId")]
        public async Task<List<StockRating>> GetStockRatingsByStoreId([FromQuery] int storeId)
        {
            return await _ratingsManager.GetStockRatingsByStoreId(storeId);
        }
        
        [HttpGet]
        [Route("averageByStock")]
        public async Task<AverageStockRatings> GetAverageStockRatingByStock([FromQuery] int stockId)
        {
            return await _ratingsManager.GetAverageStockRatingByStock(stockId);
        }
        
        [HttpPost]
        [Route("averagesByStocks")]
        public async Task<List<AverageStockRatings>> GetAverageStockRatingsByStockIds([FromBody] List<int> stockIds)
        {
            return await _ratingsManager.GetAverageStockRatingsByStockIds(stockIds);
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

            return await _ratingsManager.CreateStockRating(stockRating);
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

            await _ratingsManager.DeleteStockRating(stockRatingToDelete);
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

            await _ratingsManager.DeleteStockRatingByStoreId(storeId);
        }
    }
}

