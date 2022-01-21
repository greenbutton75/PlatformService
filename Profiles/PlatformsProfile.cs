using AutoMapper;
using PlatformService.Dtos;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // src -> tgt
            CreateMap<Models.Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Models.Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
        }
    }
}