using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using StockService.Database;
using StockService.Models;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        [Route("/byStockIds")]
        public async Task<PaginatedResult> GetStocksByStockIds([FromQuery] string[] stockIds, [FromQuery] int pageNumber, [FromQuery] int itemsPerPage)
        {
            var allStocks = await DbInitializer.context.Stocks.Where(stock => stockIds.Contains(stock.Id.ToString())).ToListAsync();

            
            return PaginateResults((dynamic)allStocks, itemsPerPage, pageNumber);
        }
        
        [HttpGet]
        [Route("/byStoreIds")]
        public async Task<PaginatedResult> GetStocksByStoreId([FromQuery] int storeId, [FromQuery] int pageNumber, [FromQuery] int itemsPerPage)
        {
            var allStocks = await DbInitializer.context.Stocks.Where(stock => storeId.Equals(stock.StoreId)).ToListAsync();
            return PaginateResults((dynamic)allStocks, itemsPerPage, pageNumber);
        }

        [HttpGet]
        [Route("/onsale/{stockName}")]
        public async Task<PaginatedResult> GetOnSaleStocks([FromQuery] float minimumPriceMultiplier, string stockName, [FromQuery] int pageNumber, [FromQuery] int itemsPerPage)
        {
            var onSaleStocks = DbInitializer.context.Stocks.Select(stock => new StockForServer()
            {
                Id = stock.Id,
                Name = stock.Name,
                Currency = stock.Currency,
                Description = stock.Description,
                Details = stock.Details,
                Available = stock.Available,
                Quantity = stock.Quantity,
                CategoryId = stock.CategoryId,
                CreatedDate = stock.CreatedDate,
                ExpiryDate = stock.ExpiryDate,
                RatingsArray = JsonConvert.DeserializeObject<StockRating[]>(stock.RatingsArrayString ?? ""),
                PriceMultiplier = JsonConvert.DeserializeObject<PriceMultiplier>(stock.PriceMultiplierObjString ?? "") ?? null,
                StoreId = stock.StoreId,
            });

            onSaleStocks = onSaleStocks
                .Where(stock => stock.PriceMultiplier != null && stock.Name.Trim().ToLower().Contains(stockName.Trim().ToLower()) && stock.PriceMultiplier.DecimalMultiplier >= minimumPriceMultiplier && stock.PriceMultiplier.CreatedDate >= DateTime.UtcNow && stock.PriceMultiplier.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(stock => stock.PriceMultiplier.DecimalMultiplier);

            var onSaleStocksOutput = await onSaleStocks.Select(stock => new Stock()
            {
                Id = stock.Id,
                Name = stock.Name,
                Currency = stock.Currency,
                Description = stock.Description,
                Details = stock.Details,
                Available = stock.Available,
                Quantity = stock.Quantity,
                CategoryId = stock.CategoryId,
                CreatedDate = stock.CreatedDate,
                ExpiryDate = stock.ExpiryDate,
                RatingsArrayString = JsonConvert.SerializeObject(stock.RatingsArray),
                PriceMultiplierObjString = JsonConvert.SerializeObject(stock.PriceMultiplier),
                StoreId = stock.StoreId,
            }).ToListAsync();

            return PaginateResults((dynamic)onSaleStocksOutput, itemsPerPage, pageNumber);
        }

        [HttpPut]
        [Route("/create")]
        public async Task<Stock> CreateStock([FromBody] Stock stock)
        {
            var newStock = (await DbInitializer.context.Stocks.AddAsync(stock)).Entity;
            await DbInitializer.context.SaveChangesAsync();
            return newStock;
        }
        
        [HttpPut]
        [Route("/update/{stockId:int}")]
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
        [Route("storeId/{storeId:int}")]
        public async void DeleteByStoreId(int storeId)
        {
            DbInitializer.context.Stocks.RemoveRange(await DbInitializer.context.Stocks.Where(stock => stock.StoreId.Equals(storeId)).ToListAsync());
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
                TotalItems = unpaginatedItems.Count,
                TotalPages = (int)Math.Ceiling((float)unpaginatedItems.Count / (float)itemsPerPage) 
            };
        }
        #endregion
    }
}