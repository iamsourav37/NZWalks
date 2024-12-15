using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Utility
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {

            // Region
            CreateMap<RegionCreateDTO, Region>();
            CreateMap<RegionUpdateDTO, Region>();
            CreateMap<RegionDTO, Region>().ReverseMap();


            // Walk
            CreateMap<CreateWalkDTO, Walk>();
            CreateMap<UpdateWalkDTO, Walk>();
            CreateMap<WalkDTO, Walk>().ReverseMap();
        }
    }
}
