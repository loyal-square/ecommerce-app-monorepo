using AutoMapper;
using MonolithServer.Models;
using Newtonsoft.Json;

namespace MonolithServer
{
    public class MappingProfile: AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Stock, StockForServer>()
                .ForMember(dest => dest.PriceMultiplier, opt => opt.MapFrom((src, dest) => ConvertToPriceMultiplier(src.PriceMultiplierObjString)));
            CreateMap<StockForServer, Stock>()
                .ForMember(dest => dest.PriceMultiplierObjString, opt => opt.MapFrom((src, dest) => ConvertToPriceMultiplierString(src.PriceMultiplier)));
        }
        private static PriceMultiplier? ConvertToPriceMultiplier(string? priceMultiplierObjString)
        {
            if(priceMultiplierObjString == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<PriceMultiplier>(priceMultiplierObjString);
        }
        private static string? ConvertToPriceMultiplierString(PriceMultiplier? priceMultiplier)
        {
            if (priceMultiplier == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(priceMultiplier);
        }
    }
}
