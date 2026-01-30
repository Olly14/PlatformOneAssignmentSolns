namespace PlatformOne.Assets.Shared.Profiles;

public class EntitiesMapperProfile : Profile
{
    public EntitiesMapperProfile()
    {
        CreateMap<Asset, AssetDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
            .ForMember(dest => dest.Isin, opt => opt.MapFrom(src => src.Isin))
            .ReverseMap();
    }
}
