using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonolithServer.Database;
using MonolithServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonolithServer.Controllers
{
    [ApiController]
    [Route("api/Image")]
    public class ImagesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public ImagesController(ApiDbContext apiDbContext)
        {
            _context = apiDbContext;
        }
        [HttpGet]
        public async Task<List<Image>> GetImages([FromQuery] int[] stockIds)
        {
            //Call database method here to return a list of Image typed objects
            return await _context.Images.Where(image => stockIds.Contains(image.StockId)).ToListAsync();
        }

        [HttpPut]
        public async Task<Image> CreateImage([FromBody] Image image)
        {
            image.Id = 0;
            var newImage = (await _context.Images.AddAsync(image)).Entity;
            await _context.SaveChangesAsync();
            
            return newImage;
        }
        
        [HttpDelete]
        [Route("imageId/{imageId:int}")]
        public async void DeleteByImageId(int imageId)
        {
            _context.Images.Remove(await _context.Images.Where(img => img.Id.Equals(imageId)).FirstOrDefaultAsync() ?? new Image());
            await _context.SaveChangesAsync();
        }
        
        [HttpDelete]
        [Route("stockId/{stockId:int}")]
        public async void DeleteByStockId(int stockId)
        {
            _context.Images.RemoveRange(await _context.Images.Where(img => img.StockId.Equals(stockId)).ToListAsync());
            await _context.SaveChangesAsync();
        }
    }
}
