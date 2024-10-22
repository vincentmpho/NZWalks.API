using AutoMapper;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTOs.RegionDto;
using NZWalks.API.Model.DTOs.WalkDifficultyDto;
using NZWalks.API.Model.DTOs.WalkDto;

namespace NZWalks.API.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<WalkDifficulty, WalkDifficultyDto>().ReverseMap();
        }
    }
}
