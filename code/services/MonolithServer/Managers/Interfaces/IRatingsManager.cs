using MonolithServer.Models;

namespace MonolithServer.Managers.Interfaces;

public interface IRatingsManager
{
    public Task<List<StockRating>> GetStockRatingsByStockIds(List<int> stockIds);
    public Task<List<StockRating>> GetStockRatingsByStoreId(int storeId);
    public Task<AverageStockRatings> GetAverageStockRatingByStock(int stockId);
    public Task<List<AverageStockRatings>> GetAverageStockRatingsByStockIds(List<int>  stockIds);
    public Task<StockRating> CreateStockRating(StockRating stockRating);
    public Task DeleteStockRating(StockRating? stockRatingToDelete);
    public Task DeleteStockRatingByStoreId(int storeId);
    public Task UpdateStockRating(StockRating stockRating);




}