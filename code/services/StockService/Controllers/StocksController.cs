using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockService.Database;
using StockService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        [Route("/byStockIds")]
        public async Task<List<Stock>> GetStocksByStockIds([FromQuery] string[] stockIds)
        {
            //Call database method here to return a list of Image typed objects
            return await DbInitializer.context.Stocks.Where(stock => stockIds.Contains(stock.Id.ToString())).ToListAsync();
        }
        
        [HttpGet]
        [Route("/byStoreIds")]
        public async Task<List<Stock>> GetStocksByStoreId([FromQuery] int storeId)
        {
            //Call database method here to return a list of Image typed objects
            return await DbInitializer.context.Stocks.Where(stock => storeId.Equals(stock.StoreId)).ToListAsync();
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
    }
}