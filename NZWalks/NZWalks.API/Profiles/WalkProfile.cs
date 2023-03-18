using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalkProfile : Profile
    {
        public WalkProfile() {
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>()
                .ReverseMap();
            // WalkDifficulty from the domain doesn't directly map to the DTO. It needs a map.
            // But because it relates to the walk profile, we will just add it here.
            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>()
                .ReverseMap();
        }
    }
}
