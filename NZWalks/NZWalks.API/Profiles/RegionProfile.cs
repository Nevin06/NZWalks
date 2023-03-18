using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile() {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();
                //.ForMember(dest => dest.Id, options => options.MapFrom(src => src.RegionId));
        }
    }
}
