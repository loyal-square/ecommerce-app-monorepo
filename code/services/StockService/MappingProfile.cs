using AutoMapper;
using Newtonsoft.Json;
using StockService.Models;

namespace StockService
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Stock, StockForServer>()
                .ForMember(dest => dest.PriceMultiplier, opt => opt.MapFrom((src, dest) => ConvertToPriceMultiplier(src.PriceMultiplierObjString)))
                .ForMember(dest => dest.RatingsArray, opt => opt.MapFrom((src, dest) => ConvertToRatingsList(src.RatingsArrayString)));
            CreateMap<StockForServer, Stock>()
               .ForMember(dest => dest.PriceMultiplierObjString, opt => opt.MapFrom((src, dest) => ConvertToPriceMultiplierString(src.PriceMultiplier)))
               .ForMember(dest => dest.RatingsArrayString, opt => opt.MapFrom((src, dest) => ConvertToRatingsListString(src.RatingsArray)));

        }
        private static PriceMultiplier? ConvertToPriceMultiplier(string? priceMultiplierObjString)
        {
            if(priceMultiplierObjString == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<PriceMultiplier>(priceMultiplierObjString);
        }
        private static List<StockRating> ConvertToRatingsList(string? ratingsListArrayString)
        {
            var emptyStockRatingList = new List<StockRating>();
            if (ratingsListArrayString == null)
            {
                return emptyStockRatingList;
            } else
            {
                return JsonConvert.DeserializeObject<List<StockRating>>(ratingsListArrayString) ?? new List<StockRating>();
            }
        }
        private static string? ConvertToPriceMultiplierString(PriceMultiplier? priceMultiplier)
        {
            if (priceMultiplier == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(priceMultiplier);
        }
        private static string ConvertToRatingsListString(List<StockRating>? ratingsList)
        {
            if(ratingsList == null)
            {
                return "[]";
            }
            return JsonConvert.SerializeObject(ratingsList);
        }
    }
}
