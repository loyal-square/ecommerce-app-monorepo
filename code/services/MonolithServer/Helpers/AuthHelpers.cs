using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using MonolithServer.Database;
using MonolithServer.Models;
using Newtonsoft.Json;

namespace MonolithServer.Helpers;

public class AuthHelpers
{
    public static async Task<bool> AccessingRestrictedStockData(ClaimsPrincipal? userToken, Stock stock, ApiDbContext context)
    {
        if (userToken == null)
        {
            // Handle the case where the Authorization header is missing
            return true;
        }
        
        //get the profile that matches the store id in stock
        var storeId = stock.StoreId;
        var allprofiles = await context.Profiles.ToListAsync();
        var matchingProfiles = allprofiles.Select(profile => new ProfileKeyInfo()
        {
            StoreIdArray = JsonConvert.DeserializeObject<List<int>>(profile.StoreIdArrayString),
            Username = profile.Username
        }).Where(info => info.StoreIdArray.Contains(storeId)).Select(x => x.Username);

        return !matchingProfiles.Contains(userToken.FindFirst("username").Value);
    }

    public static async Task<bool> AccessingRestrictedRatingsData(ClaimsPrincipal? userToken, StockRating stockRating, ApiDbContext context)
    {
        if (userToken == null)
        {
            // Handle the case where the Authorization header is missing
            return true;
        }

        var user = await context.Profiles.FindAsync(stockRating.UserId);
        return !user?.Username.Equals(userToken.FindFirst("username").Value) ?? true;
    }

    public static async Task<bool> AccessingRestrictedDataByStoreId(ClaimsPrincipal? userToken, int storeId, ApiDbContext context)
    {
        if (userToken == null)
        {
            // Handle the case where the Authorization header is missing
            return true;
        }
        
        var profiles = await context.Profiles.ToListAsync();
        var matchingProfiles = profiles.Select(profile => new ProfileKeyInfo()
        {
            StoreIdArray = JsonConvert.DeserializeObject<List<int>>(profile.StoreIdArrayString),
            Username = profile.Username
        }).Where(info => info.StoreIdArray.Contains(storeId)).Select(x => x.Username);
        
        return !matchingProfiles.Contains(userToken.FindFirst("username").Value);
    }
}

public class ProfileKeyInfo
{
    public List<int>? StoreIdArray { get; set; }
    public string Username { get; set; }
}