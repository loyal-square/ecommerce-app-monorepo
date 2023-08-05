﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EdgeService.Models;

namespace EdgeService.Controllers;

[ApiController]
[Route("api/Edge/StockService")]
public class StockServiceController
{
    private HttpClient _httpClient;
    private readonly IMapper _mapper;
    public StockServiceController(IMapper mapper)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5089/api");
        _mapper = mapper;
    }

    [HttpGet]
    [Route("stocks/all/filters")]
    public async Task<PaginatedResult> GetAllStocksWithFilters([FromQuery] bool? ascendingNames,
        [FromQuery] bool? ascendingPrices, [FromQuery] bool? ascendingCreatedDates, [FromQuery] bool? onlyAvailable,
        [FromQuery] int? minimumQuantity, [FromQuery] int? pageNumber,
        [FromQuery] int? itemsPerPage)
    {
        var responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Stock/all/filters?itemsPerPage={itemsPerPage}&pageNumber={pageNumber}&ascendingNames={ascendingNames}&ascendingPrices={ascendingPrices}&ascendingCreatedDates={ascendingCreatedDates}&minimumQuantity={minimumQuantity}&onlyAvailable={onlyAvailable}");
        var returnObject = await responseMessage.Content.ReadFromJsonAsync<PaginatedResult>();
        return returnObject ?? new PaginatedResult();
    }

    [HttpGet]
    [Route("stocks/byStockIds")]
    public async Task<PaginatedResult> GetStocksByStockIds([FromQuery] string[] stockIds,
        [FromQuery] int? pageNumber, [FromQuery] int? itemsPerPage)
    {
        pageNumber ??= 1;
        itemsPerPage ??= 10;
        var stockIdsQueryString = "";
        foreach (var stockId in stockIds.Select((value, i) => new { i, value }))
        {
            var value = stockId.value;
            var index = stockId.i;
            if (index.Equals(0))
            {
                stockIdsQueryString += $"stockIds={value}";
            }
            else
            {
                stockIdsQueryString += $"&stockIds={value}";
            }
        }
        var responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Stock/byStockIds?{stockIdsQueryString}&itemsPerPage={itemsPerPage}&pageNumber={pageNumber}");
        var returnObject = await responseMessage.Content.ReadFromJsonAsync<PaginatedResult>();
        return returnObject ?? new PaginatedResult();
    }

    [HttpGet]
    [Route("stocks/byStoreId")]
    public async Task<PaginatedResult> GetStocksByStoreId([FromQuery] int storeId, [FromQuery] int? pageNumber,
        [FromQuery] int? itemsPerPage)
    {
        pageNumber ??= 1;
        itemsPerPage ??= 10;
        
        var responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Stock/byStoreId?storeId={storeId}&itemsPerPage={itemsPerPage}&pageNumber={pageNumber}");
        var returnObject = await responseMessage.Content.ReadFromJsonAsync<PaginatedResult>();
        return returnObject ?? new PaginatedResult();
    }
    /*[HttpGet]
    [Route("onsale")]
    
    [HttpPut]
    [Authorize]
    [Route("create")]
    
    [HttpPut]
    [Authorize]
    [Route("update/{stockId:int}")]
    
    [HttpDelete]
    [Authorize]
    [Route("stockId/{stockId:int}")]
    
    [HttpDelete]
    [Authorize]
    [Route("storeId/{storeId:int}")]*/
}