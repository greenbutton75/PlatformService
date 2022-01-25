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

            CreateMap<Models.Platform,GrpcPlatformModel>()
                .ForMember (dest => dest.PlatformId, opt => opt.MapFrom (src => src.Id))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.Publisher, opt => opt.MapFrom (src => src.Publisher));
        }
    }
}