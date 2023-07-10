using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockService.Database;
using StockService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        // GET: api/<ImagesController>
        [HttpGet]
        public async Task<List<Images>> GetImagesS3([FromQuery] string[] stockIds)
        {
            //Call database method here to return a list of Image typed objects
            return await DbInitializer.context.Images.Where(image => stockIds.Contains(image.StockId.ToString())).ToListAsync();
        }

        // POST api/<ImagesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ImagesController>/5
        [HttpPut]
        public void Put([FromRoute] int id, [FromBody] string value)
        {
        }

        // DELETE api/<ImagesController>/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
