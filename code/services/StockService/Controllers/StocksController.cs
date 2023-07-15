using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockService.Database;
using StockService.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockService.Controllers
{
    [ApiController]
    [Route("api/Stock")]
    public class StockController : ControllerBase
    {
        private readonly IMapper _mapper;

        public StockController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("all/filters")]
        public async Task<PaginatedResult> GetAllStocksWithFilters([FromQuery] bool? ascendingNames,
            [FromQuery] bool? ascendingPrices, [FromQuery] bool? ascendingCreatedDates, [FromQuery] bool? onlyAvailable,
            [FromQuery] int? minimumQuantity, [FromQuery] int? pageNumber,
            [FromQuery] int? itemsPerPage)
        {
            var allStocks = await DbInitializer.context.Stocks.ToListAsync();
            minimumQuantity ??= 0;
            var filteredStockList = new List<Stock>();
            foreach (var stock in allStocks.Where(stock => stock.Quantity >= minimumQuantity))
            {
                if (onlyAvailable == true)
                {
                    if (stock.Available) filteredStockList.Add(stock);
                }
                else
                {
                    filteredStockList.Add(stock);
                }
            }

            var stocksForServer = _mapper.Map<List<StockForServer>>(filteredStockList);

            var filteredStocksForServer = new List<StockForServer>();
            if (ascendingNames == true) filteredStocksForServer = stocksForServer.OrderBy(stock => stock.Name).ToList();
            else if (ascendingPrices == true)
            {
                foreach (var stockForServer in stocksForServer)
                {
                    var applyPriceMultiplier = stockForServer.PriceMultiplier != null &&
                                               stockForServer.PriceMultiplier.CreatedDate < DateTime.UtcNow &&
                                               stockForServer.PriceMultiplier.ExpiryDate > DateTime.UtcNow;
                    if (applyPriceMultiplier)
                    {
                        filteredStocksForServer = stocksForServer
                            .OrderBy(stock => stock.Price * stock.PriceMultiplier?.DecimalMultiplier).ToList();
                    }
                    else
                    {
                        filteredStocksForServer = stocksForServer.OrderBy(stock => stock.Price).ToList();
                    }
                }
            }
            else if (ascendingCreatedDates == true)
            {
                filteredStocksForServer = stocksForServer.OrderBy(stock => stock.CreatedDate).ToList();
            }
            else
            {
                filteredStocksForServer = stocksForServer;
            }

            itemsPerPage ??= 10;
            pageNumber ??= 1;

            var stocksForClient = _mapper.Map<List<Stock>>(filteredStocksForServer);

            return PaginateResults((dynamic)stocksForClient, (int)itemsPerPage, (int)pageNumber);
        }

        [HttpGet]
        [Route("byStockIds")]
        public async Task<PaginatedResult> GetStocksByStockIds([FromQuery] string[] stockIds,
            [FromQuery] int pageNumber, [FromQuery] int itemsPerPage)
        {
            var allStocks = await DbInitializer.context.Stocks.Where(stock => stockIds.Contains(stock.Id.ToString()))
                .ToListAsync();


            return PaginateResults((dynamic)allStocks, itemsPerPage, pageNumber);
        }

        [HttpGet]
        [Route("byStoreId")]
        public async Task<PaginatedResult> GetStocksByStoreId([FromQuery] int storeId, [FromQuery] int pageNumber,
            [FromQuery] int itemsPerPage)
        {
            var allStocks = await DbInitializer.context.Stocks.Where(stock => storeId.Equals(stock.StoreId))
                .ToListAsync();
            return PaginateResults((dynamic)allStocks, itemsPerPage, pageNumber);
        }

        [HttpGet]
        [Route("onsale")]
        public async Task<PaginatedResult> GetOnSaleStocks([FromQuery] float? minimumPriceMultiplier,
            [FromQuery] string? stockName, [FromQuery] int? pageNumber, [FromQuery] int? itemsPerPage,
            [FromQuery] float? minimumAverageRating)
        {
            stockName ??= "";
            minimumPriceMultiplier ??= 1;
            pageNumber ??= 1;
            itemsPerPage ??= 10;

            var onSaleStocks = _mapper.Map<List<StockForServer>>(DbInitializer.context.Stocks);

            onSaleStocks = onSaleStocks
                .Where(stock => stock.Available == true && stock.Quantity > 0 && stock.ExpiryDate > DateTime.UtcNow &&
                                stock.Name.Trim().ToLower().Contains(stockName.Trim().ToLower()))
                .Where(stock =>
                    stock.PriceMultiplier != null &&
                    stock.PriceMultiplier.DecimalMultiplier <= minimumPriceMultiplier &&
                    stock.PriceMultiplier.CreatedDate <= DateTime.UtcNow &&
                    stock.PriceMultiplier.ExpiryDate > DateTime.UtcNow)
                .OrderBy(stock => stock.PriceMultiplier?.DecimalMultiplier).ToList();

            Console.WriteLine(onSaleStocks.ToString());

            var onSaleStocksOutput = _mapper.Map<List<Stock>>(onSaleStocks);
            minimumAverageRating ??= 0;
            onSaleStocksOutput = await FilterByAverageRating(onSaleStocksOutput, (float)minimumAverageRating);
            return PaginateResults((dynamic)onSaleStocksOutput, (int)itemsPerPage, (int)pageNumber);
        }

        [HttpPut]
        [Authorize]
        [Route("create")]
        public async Task<Stock> CreateStock([FromBody] Stock stock)
        {
            stock.Id = 0;
            var newStock = (await DbInitializer.context.Stocks.AddAsync(stock)).Entity;
            await DbInitializer.context.SaveChangesAsync();
            var newStockForClient = _mapper.Map<Stock>(newStock);
            return newStockForClient;
        }

        [HttpPut]
        [Authorize]
        [Route("update/{stockId:int}")]
        public async Task<Stock> UpdateStock([FromBody] Stock stockValues, int stockId)
        {
            var stockToUpdate = await DbInitializer.context.Stocks.FindAsync(stockId);
            DbInitializer.context.Stocks.Remove(stockToUpdate);
            await DbInitializer.context.SaveChangesAsync();
            stockValues.Id = stockId;
            var updatedStock = await DbInitializer.context.Stocks.AddAsync(stockValues);
            await DbInitializer.context.SaveChangesAsync();
            return updatedStock.Entity;
        }

        [HttpDelete]
        [Authorize]
        [Route("stockId/{stockId:int}")]
        public async void DeleteByStockId(int stockId)
        {
            var stockToDelete = await DbInitializer.context.Stocks.FindAsync(stockId);
            if (stockToDelete != null)
            {
                DbInitializer.context.Stocks.Remove(stockToDelete);
                await DbInitializer.context.SaveChangesAsync();
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("storeId/{storeId:int}")]
        public async void DeleteByStoreId(int storeId)
        {
            DbInitializer.context.Stocks.RemoveRange(await DbInitializer.context.Stocks
                .Where(stock => stock.StoreId.Equals(storeId)).ToListAsync());
            await DbInitializer.context.SaveChangesAsync();
        }

        #region private

        private PaginatedResult PaginateResults(dynamic unpaginatedItems, int itemsPerPage, int pageNumber)
        {
            dynamic paginatedItems = new List<dynamic>();
            var currentPage = 1;
            var currentItemCounter = 1;

            for (var i = 0; i < unpaginatedItems.Count; i++)
            {
                if (currentItemCounter <= itemsPerPage)
                {
                    if (currentPage == pageNumber)
                    {
                        paginatedItems.Add(unpaginatedItems[i]);
                    }

                    currentItemCounter++;
                }
                else
                {
                    i--;
                    currentPage++;
                    currentItemCounter = 1;
                }
            }

            return new PaginatedResult()
            {
                PaginatedItems = paginatedItems,
                TotalItems = unpaginatedItems?.Count ?? 0,
                TotalPages = (int)Math.Ceiling((unpaginatedItems?.Count ?? 0) / (float)itemsPerPage)
            };
        }

        private async Task<List<Stock>> FilterByAverageRating(List<Stock> allStocks, float minimumAverageRating)
        {
            var stockRatings = await DbInitializer.context.StockRatings
                .Where(rating => rating.RatingValue >= minimumAverageRating).ToListAsync();
            var matchingStocks = new List<Stock>();
            for (var i = 0; i < stockRatings.Count; i++)
            {
                var stockRating = stockRatings[i];
                var matchingStock = allStocks.Find(stock => stock.Id.Equals(stockRating.StockId));
                if (matchingStock != null) matchingStocks.Add(matchingStock);
            }

            return matchingStocks;
        }

        #endregion
    }
}