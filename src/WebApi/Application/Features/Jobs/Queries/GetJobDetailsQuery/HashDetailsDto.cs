using AutoMapper;
using LaserPointer.WebApi.Application.Common.Mappings;
using LaserPointer.WebApi.Domain;
using LaserPointer.WebApi.Domain.Entities;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetJobDetailsQuery
{
    public class HashDetailsDto : IMapFrom<Hash>
    {
		public int Id { get; set; }
		public string HexValue { get; set; }
        public string PlainValue { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Hash, HashDetailsDto>()
                .ForMember(dest => dest.HexValue,
                    opt => opt.MapFrom(e => e.AsHexString())
                );

        }
    }
}
