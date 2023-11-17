using MonolithServer.Models;

namespace MonolithServer.Managers.Interfaces;

public interface IStockManager
{
    public Task<PaginatedResult> GetAllStocksWithFilters(bool? ascendingNames,
        bool? ascendingPrices, bool? ascendingCreatedDates, bool? onlyAvailable,
        int? minimumQuantity, int? pageNumber,
        int? itemsPerPage);
    public Task<PaginatedResult> GetStocksByStockIds(string[] stockIds, int pageNumber, int itemsPerPage);
    public Task<PaginatedResult> GetStocksByStoreId(int storeId, int pageNumber, int itemsPerPage);
    public Task<PaginatedResult> GetOnSaleStocks(float? minimumPriceMultiplier,
        string? stockName, int? pageNumber, int? itemsPerPage,
        float? minimumAverageRating);
    public Task<Stock> CreateStock(Stock stock);
    public Task<Stock> UpdateStock(Stock stockValues, int stockId);
    public Task DeleteByStockId(Stock stock);
    public Task DeleteByStoreId(int storeId);
}